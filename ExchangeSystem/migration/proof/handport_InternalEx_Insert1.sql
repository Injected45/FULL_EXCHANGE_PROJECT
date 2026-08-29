-- =====================================================================================
-- Hand-port: InternalEx_Insert1  (four T-SQL forward jumps -> flags; MySQL has no such statement)
--
-- FOUR forward jumps, each leaving its branch and landing inside a LATER branch:
--   jump             guard enclosing the LABEL     what the jump skips
--   Confirm          IF @ConfirmType = 0           the whole 'save' body
--   Delivered        IF @IsAccTo     = 0           the destination-changed checks
--   CancleConfirm    IF @ConfirmType = 5           the variable-assignment region
--   DeliveredCancle  IF @ConfirmType = 6           nothing (label is first in the block)
--
-- Each rewritten mechanically and identically:
--   GOTO X         ->  SET v_goto_X = 1;   (then fall out of the current branch)
--   IF <guard>     ->  IF (<guard> OR v_goto_X = 1)     so the flag can enter that branch
--   <skipped body> ->  wrapped in IF v_goto_X = 0 THEN .. END IF;   (omitted when empty)
--   X:             ->  removed
--
-- All 12 anchors (4 jumps + 4 labels + 4 guards) were asserted against the converter output before
-- editing. Block nesting was computed with CASE..END accounted for, so the guard picked for each
-- label is the block that genuinely encloses it.
--
-- Also fixed: the converter does not treat GOTO as a statement boundary, so the statement right
-- before a jump (e.g. 'SET @ID = 0') was emitted WITHOUT its ';'. Two such statements terminated.
--
-- Behaviour deliberately PRESERVED, including the spaghetti: 'Delivered' jumps INTO the
-- IF @IsAccTo = 0 block even when @IsAccTo = 1, then falls through to the following
-- IF @IsAccTo = 1 which overwrites @SafeOrAcc. That fall-through is kept exactly.
--
-- !! NEEDS A LIVE TEST BEFORE IT IS TRUSTED. This is the internal-transfer ledger proc. Static
-- !! checks prove the SHAPE is right; they cannot prove the money is right. Run one transfer per
-- !! path (save / confirm / deliver / cancel-confirm / deliver-cancelled) and compare the
-- !! resulting AccSafeActivityTb rows against SQL Server.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `InternalEx_Insert1`;
DELIMITER $$
CREATE PROCEDURE `InternalEx_Insert1`(IN `p_Code` LONGTEXT, IN `p_SenderName` LONGTEXT, IN `p_SPhone1` LONGTEXT, IN `p_SPhone2` LONGTEXT, IN `p_SenderIDNo` LONGTEXT, IN `p_RecievedName` LONGTEXT, IN `p_RPhone1` LONGTEXT, IN `p_RPhone2` LONGTEXT, IN `p_RecievedIDNo` LONGTEXT, IN `p_RecievedCurrencyID` INT, IN `p_DeliveredCurrencyID` INT, IN `p_OverallVal` DECIMAL(12,3), IN `p_ExVal` DECIMAL(12,3), IN `p_SafeID` INT, IN `p_DeliveryPlace` INT, IN `p_BranchRecievedID` INT, IN `p_BranchDeliveredID` INT, IN `p_ConfirmType` INT, IN `p_Notes` LONGTEXT, INOUT `p_msgIN` INT, INOUT `p_MSG` LONGTEXT, IN `p_IDCode` BIGINT, IN `p_IsAccFrom` TINYINT UNSIGNED, IN `p_AccIDFrom` BIGINT, IN `p_IsAccTo` TINYINT UNSIGNED, IN `p_AccIDTo` BIGINT, IN `p_BBRANCHID` INT, IN `p_EXTRVAL` DECIMAL(12,3), IN `p_ServiceType` INT, IN `p_ISHandallEX` TINYINT(1), IN `p_HandallExVal` DECIMAL(12,3), IN `p_HandallExVal2` DECIMAL(12,3))
proc: BEGIN
DECLARE v_goto_Confirm INT DEFAULT 0;
DECLARE v_goto_Delivered INT DEFAULT 0;
DECLARE v_goto_CancleConfirm INT DEFAULT 0;
DECLARE v_goto_DeliveredCancle INT DEFAULT 0;
DECLARE v_ID BIGINT;
DECLARE v_RBShare           DECIMAL(12, 3);
DECLARE v_DBShare           DECIMAL(12, 3);
DECLARE v_MainShare DECIMAL(12, 3);
DECLARE v_RBCurrnetVal DECIMAL(12, 3);
DECLARE v_DBCurrnetVal DECIMAL(12, 3);
DECLARE v_SafeOrAcc BIGINT;
DECLARE v_BBRANCHAccID BIGINT;
DECLARE v_AccBranchID INT;
DECLARE v_CanDepit TINYINT;
DECLARE v_IsLimted TINYINT;
DECLARE v_LimtedVal DECIMAL(18, 3);
DECLARE v_OldConfType INT;
DECLARE v_IsCanceled INT;
DECLARE v_OldBranchDel INT;
DECLARE v_OldCurrentDel INT;
DECLARE v_IsBranchLimited BIT;
DECLARE v_BranchLimitedVal DECIMAL(18, 3);
DECLARE v_TotalAllowedLimit DECIMAL(18, 3);
DECLARE v_MobileType INT;
DECLARE v_BBRANCHALLOWACCID BIGINT;
DECLARE v_IsConfirmEdit BIT DEFAULT CASE                    WHEN p_ConfirmType IN (1, 11)                      THEN                      1                    ELSE                    0            END;
DECLARE v_MovementType LONGTEXT;
DECLARE v_MovementUserTypeTo LONGTEXT;
DECLARE v_RBrType INT;
DECLARE v_DBrType INT;
DECLARE v_OldDBrType INT;
DECLARE v_AccFrom BIGINT;
DECLARE v_AccUserID BIGINT;
DECLARE v_AccTo BIGINT;
DECLARE v_AccForDel BIGINT;
DECLARE v_AccAgentForDel BIGINT;
DECLARE v_MainBr INT;
DECLARE v_AccIncome BIGINT;
DECLARE v_AccCancleIncome BIGINT;
DECLARE v_CurrentDelAccID BIGINT;
DECLARE v_CurrentAccID BIGINT;
DECLARE v_AccMainFromBranches BIGINT;
DECLARE v_AccLossMainFromBranches BIGINT;
DECLARE v_NetTotla DECIMAL(12, 3);
DECLARE v_MainAcccurrent BIGINT;
DECLARE v_AccOutcome BIGINT;
DECLARE v_OldAccOutcome BIGINT;
DECLARE v_AccLossOutcome BIGINT;
DECLARE v_OldAccLossOutcome BIGINT;
DECLARE v_BRMAINDES BIGINT;
DECLARE v_BRRECDES BIGINT;
DECLARE v_FSBRRATELEFT DECIMAL(12, 3);
DECLARE v_AgentMovement LONGTEXT;
DECLARE v_BRRECNAME LONGTEXT;
DECLARE v_FIRSTBRRATE DECIMAL(12, 3);
DECLARE v_SECOBRRATE DECIMAL(12, 3);
DECLARE v_DBRNAME VARCHAR(50);
DECLARE v_CityName VARCHAR(200);
DECLARE v_AccCancelRB BIGINT;
DECLARE v_SumDebit DECIMAL(18,3);
DECLARE v_SumCredit DECIMAL(18,3);
-- The T-SQL CATCH is:  ROLLBACK TRANSACTION; SET @msgIN = 0; SET @MSG = ERROR_MESSAGE()
-- It does NOT re-raise — it RETURNS the error to the caller through the two OUTPUT parameters, and the
-- app reads @MSG to decide what to show. The handler must therefore assign them; a bare ROLLBACK/RESIGNAL
-- would surface an exception instead and leave @MSG holding whatever the caller passed in.
-- GET DIAGNOSTICS runs BEFORE the ROLLBACK so the message cannot be lost with the transaction.
DECLARE EXIT HANDLER FOR SQLEXCEPTION
BEGIN
    DECLARE __err_no BIGINT DEFAULT 0;
    DECLARE __err_msg VARCHAR(512) DEFAULT '';
    GET DIAGNOSTICS CONDITION 1 __err_no = MYSQL_ERRNO, __err_msg = MESSAGE_TEXT;
    ROLLBACK;
    SET p_msgIN = 0;
    SET p_MSG   = __err_msg;
END;
      START TRANSACTION;




      IF p_ConfirmType <> 0
         THEN


          SELECT a.IsAccFrom, a.IsAccTo, a.AccFrom, a.AccTo, a.BranchRecievedID, a.RecievedCurrencyID, a.DeliveredCurrencyID, a.BranchDeliveredID, CASE                                                  WHEN v_IsConfirmEdit = 1                                                    THEN                                                    p_BranchDeliveredID                                                  ELSE                                                  a.BranchDeliveredID                                          END, CASE                                                  WHEN v_IsConfirmEdit = 1                                                    THEN                                                    p_OverallVal                                                  ELSE                                                  a.OverallVal                                          END, CASE                                                  WHEN v_IsConfirmEdit = 1                                                    THEN                                                    p_ExVal                                                  ELSE                                                  a.ExVal                                          END, a.RecievedName, a.RPhone1, a.SenderName, a.SPhone1, a.Type_Moble INTO p_IsAccFrom, p_IsAccTo, p_AccIDFrom, p_AccIDTo, p_BranchRecievedID, p_RecievedCurrencyID, p_DeliveredCurrencyID, v_OldBranchDel, p_BranchDeliveredID, p_OverallVal, p_ExVal, p_RecievedName, p_RPhone1, p_SenderName, p_SPhone1, v_MobileType FROM
                  InternalEx a
          WHERE
                  a.Code = p_Code;

        END IF;







      SELECT BranchType INTO v_RBrType FROM
              CoBranch
      WHERE
              ID = p_BranchRecievedID;
      SELECT BranchType INTO v_DBrType FROM
              CoBranch
      WHERE
              ID = p_BranchDeliveredID;
      SELECT BranchType INTO v_OldDBrType FROM
              CoBranch
      WHERE
              ID = v_OldBranchDel;

      SELECT AccID INTO v_AccFrom FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 2010601;

      SELECT AccID INTO v_AccUserID FROM
              TB_Users
      WHERE
              USID = p_SafeID;


      SELECT a.AccID INTO v_BBRANCHALLOWACCID FROM
              AccountsTb AS a
      WHERE
              a.BranchID = p_BranchRecievedID
              AND a.AccParent = 40101018;


      SELECT bt.AccID INTO v_BBRANCHAccID FROM
              BBranchTb bt
      WHERE
              bt.ID = p_BBRANCHID;

      SELECT AccID INTO v_AccTo FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 1010601;

      SELECT AccID INTO v_AccForDel FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 2010605;

      SELECT AccID INTO v_AccAgentForDel FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 2010607;

      SELECT ID INTO v_MainBr FROM
              CoBranch
      WHERE
              IsMain = 1;

      SELECT AccID INTO v_AccIncome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 4010102;

      SELECT AccID INTO v_AccCancleIncome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 3010209;

      SELECT CurrentAccID INTO v_CurrentDelAccID FROM
              CoBranch cb
      WHERE
              ID = p_BranchDeliveredID;


      SELECT CurrentAccID INTO v_OldCurrentDel FROM
              CoBranch cb
      WHERE
              ID = v_OldBranchDel;

      SELECT CurrentAccID INTO v_CurrentAccID FROM
              CoBranch cb
      WHERE
              ID = p_BranchRecievedID;

      SELECT AccID INTO v_AccMainFromBranches FROM
              AccountsTb
      WHERE
              AccountsTb.AccCode = 401010901;

      SELECT AccID INTO v_AccLossMainFromBranches FROM
              AccountsTb
      WHERE
              AccountsTb.AccCode = 301020701;

SET v_NetTotla = p_OverallVal + p_ExVal;
      SELECT CurrentAccID INTO v_MainAcccurrent FROM
              CoBranch cb
      WHERE
              ID = v_MainBr;

      SELECT AccID INTO v_AccOutcome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 4010103;

      SELECT AccID INTO v_OldAccOutcome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = v_OldBranchDel
              AND AccountsTb.AccParent = 4010103;

      SELECT AccID INTO v_AccLossOutcome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 3010206;

      SELECT AccID INTO v_OldAccLossOutcome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = v_OldBranchDel
              AND AccountsTb.AccParent = 3010206;

      SELECT at.AccID INTO v_BRMAINDES FROM
              AccountsTb at
      WHERE
              at.AccParent = 2010701
              AND at.BranchID = v_MainBr;

      SELECT at.AccID INTO v_BRRECDES FROM
              AccountsTb at
      WHERE
              at.AccParent = 2010701
              AND at.BranchID = p_BranchDeliveredID;



      -- BranchRatesTb is NOT unique on (FBranchID, SBranchID): 46 pairs have two rows today, and for 33
      -- of them the rates DIFFER (branch 38's rows are duplicated as 0.000 and 50.000). T-SQL tolerates
      -- the multi-row assignment and keeps the LAST row; MySQL raises ERROR 1172 and the whole transfer
      -- aborts. Measured against SQL Server on pairs (38,2), (38,14) and (38,39): it returns the
      -- HIGHEST-ID row (50.000), so ORDER BY a.ID DESC LIMIT 1 reproduces it and is deterministic.
      SELECT a.FBRate INTO v_FIRSTBRRATE FROM
              BranchRatesTb AS a
      WHERE
              a.FBranchID = p_BranchRecievedID
              AND a.SBranchID = p_BranchDeliveredID
      ORDER BY a.ID DESC LIMIT 1;
      SELECT a.SBRate INTO v_SECOBRRATE FROM
              BranchRatesTb AS a
      WHERE
              a.SBranchID = p_BranchDeliveredID
              AND a.FBranchID = p_BranchRecievedID
      ORDER BY a.ID DESC LIMIT 1;

      SELECT BName INTO v_DBRNAME FROM
              CoBranch cb
      WHERE
              ID = p_BranchDeliveredID;



      SELECT cb.BName INTO v_BRRECNAME FROM
              CoBranch cb
      WHERE
              cb.ID = p_BranchRecievedID;



      SELECT c.CityName INTO v_CityName FROM
              CitiesTb AS c
      WHERE
              c.ID = p_DeliveryPlace;



      SELECT AccID INTO v_AccCancelRB FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 2010602;
SET v_RBShare = CASE                                  WHEN p_BranchRecievedID = p_BranchDeliveredID                                    AND p_BranchRecievedID = v_MainBr                                    THEN                                    p_ExVal                                  WHEN p_BranchRecievedID = p_BranchDeliveredID                                    AND p_BranchRecievedID <> v_MainBr                                    THEN                                    p_ExVal / 4                                  ELSE                                  IFNULL(p_ExVal / 100 * v_FIRSTBRRATE, 0)                          END;
SET v_DBShare = CASE                                  WHEN p_BranchRecievedID = p_BranchDeliveredID                                    AND p_BranchRecievedID = v_MainBr                                    THEN                                    0                                  WHEN p_BranchRecievedID = p_BranchDeliveredID                                    AND p_BranchRecievedID <> v_MainBr                                    THEN                                    p_ExVal / 4                                  ELSE                                  IFNULL(p_ExVal / 100 * v_SECOBRRATE, 0)                          END;
      SET v_MainShare = IFNULL(p_ExVal - v_RBShare - v_DBShare, 0);
      SET v_RBCurrnetVal = IFNULL(v_NetTotla - v_RBShare, 0);
      SET v_DBCurrnetVal = IFNULL(p_OverallVal + v_DBShare, 0);
      IF p_ConfirmType = 11
         THEN

          IF v_OldBranchDel = p_BranchDeliveredID
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'الرجاء تغيير الوجهة أولا ليتم الحفظ';
              ROLLBACK;
              LEAVE proc;
            END IF;



          IF p_IsAccFrom = 0
             THEN
              SET v_SafeOrAcc = v_AccUserID;
            END IF;
          IF p_IsAccFrom = 1
             THEN
              SET v_SafeOrAcc = p_AccIDFrom;
            END IF;
          IF p_IsAccFrom = 2
             THEN
              SET v_SafeOrAcc = v_BBRANCHAccID;
            END IF;







          IF v_OldDBrType = 3
            AND
            v_RBrType <> 3
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, v_NetTotla, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_AccFrom, v_SafeOrAcc, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;


          INSERT INTO ExSyAccounts_AccSafeActivityTb
                (
                  SafeID,
                  Debit,
                  Credit,
                  InsertDate,
                  ISID,
                  TypeID,
                  OperationTypeID,
                  AccBranchID,
                  AccIDFrom,
                  AccIDTo,
                  MovementType,
                  CurrencyID,
                  Note,
                  SafeIDMovement
                )
              SELECT
                      p_SafeID,
                      a.Credit,
                      a.Debit,
                      NOW(),
                      a.ISID,
                      1,
                      7,
                      a.AccBranchID,
                      a.AccIDFrom,
                      CASE                                WHEN p_BranchDeliveredID = AccBranchID                                  OR v_RBrType = 3                                  THEN                                  a.AccIDTo                                ELSE                                v_AccCancelRB                        END,
                      'حوالة ملغاة',
                      p_RecievedCurrencyID,
                      p_Notes,
                      'حوالة ملغاة'
              FROM
                      ExSyAccounts_AccSafeActivityTb a
              WHERE
                      a.ISID = p_Code
                      AND (   a.OperationTypeID = 4
                              OR a.OperationTypeID = 3
                              OR a.OperationTypeID = 94);


          IF EXISTS (SELECT
                        1
                FROM
                        ExSyAccounts_AccSafeActivityTb a
                WHERE
                        a.ISID = p_Code
                        AND a.AccIDFrom = v_AccIncome
                        AND a.Debit = 0
                        AND a.Credit - a.Debit <> 0
                        AND a.OperationTypeID <> 7)
             THEN
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
                  SELECT p_SafeID,
                          a.Credit,
                          a.Debit,
                          NOW(),
                          a.ISID,
                          1,
                          25,
                          a.AccBranchID,
                          v_AccCancleIncome,
                          v_AccCancelRB,
                          v_MovementType,
                          p_RecievedCurrencyID,
                          p_Notes,
                          v_MovementType
                  FROM
                          ExSyAccounts_AccSafeActivityTb a
                  WHERE
                          a.ISID = p_Code
                          AND a.AccIDFrom = v_AccIncome
                          AND a.Debit = 0
                  ORDER BY
                          a.ID DESC LIMIT 1 ;
            END IF;


          IF EXISTS (SELECT
                        1
                FROM
                        ExSyAccounts_AccSafeActivityTb a
                WHERE
                        a.ISID = p_Code
                        AND a.AccIDFrom = v_OldAccOutcome
                        AND a.Debit = 0
                        AND a.Credit - a.Debit <> 0
                        AND a.OperationTypeID <> 7)
             THEN
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
                  SELECT p_SafeID,
                          a.Credit,
                          a.Debit,
                          NOW(),
                          a.ISID,
                          1,
                          25,
                          a.AccBranchID,
                          v_OldAccLossOutcome,
                          v_AccAgentForDel,
                          v_MovementType,
                          p_RecievedCurrencyID,
                          p_Notes,
                          v_MovementType
                  FROM
                          ExSyAccounts_AccSafeActivityTb a
                  WHERE
                          a.ISID = p_Code
                          AND a.AccIDFrom = v_OldAccOutcome
                          AND a.Debit = 0
                  ORDER BY
                          a.ID DESC LIMIT 1 ;
            END IF;


          IF EXISTS (SELECT
                        1
                FROM
                        ExSyAccounts_AccSafeActivityTb a
                WHERE
                        a.ISID = p_Code
                        AND a.AccIDFrom = v_AccMainFromBranches
                        AND a.Debit = 0
                        AND a.Credit - a.Debit <> 0
                        AND a.OperationTypeID <> 7)
             THEN
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
                  SELECT p_SafeID,
                          a.Credit,
                          a.Debit,
                          NOW(),
                          a.ISID,
                          1,
                          25,
                          a.AccBranchID,
                          v_AccLossMainFromBranches,
                          v_CurrentAccID,
                          v_MovementType,
                          p_RecievedCurrencyID,
                          p_Notes,
                          v_MovementType
                  FROM
                          ExSyAccounts_AccSafeActivityTb a
                  WHERE
                          a.ISID = p_Code
                          AND a.AccIDFrom = v_AccMainFromBranches
                          AND a.Debit = 0
                  ORDER BY
                          a.ID DESC LIMIT 1 ;
            END IF;


          UPDATE
                  ExSyAccounts_AccSafeActivityTb
          SET
                  OperationTypeID = 7
          WHERE
            ISID = p_Code;
          SET v_ID = 0;
          SET v_goto_Confirm = 1;   -- jump replaced by flag

        END IF;

      IF p_ConfirmType = 0
         THEN
          IF v_goto_Confirm = 0 THEN   -- skipped when jumping to Confirm


          SET v_MovementType = CONCAT('مرسلة إلى/ ', IFNULL(p_RecievedName, ''));
          IF p_IsAccFrom = 0
             THEN
              SET v_SafeOrAcc = v_AccUserID;
            END IF;
          IF p_IsAccFrom = 1
             THEN
              SET v_SafeOrAcc = p_AccIDFrom;
              SET v_MovementType = CONCAT('محولة من حساب/ ', p_SenderName);
            END IF;
          IF p_IsAccFrom = 2
             THEN
              SET v_SafeOrAcc = v_BBRANCHAccID;
            END IF;







          IF EXISTS (SELECT
                        1
                FROM
                        InternalEx
                WHERE
                        IDCode = p_IDCode)
             THEN
              SET p_msgIN = 2;
              SET p_MSG = N'عذراً هذا الرقم موجود مسبقاً';
              ROLLBACK;
              LEAVE proc;
            END IF;
















          IF p_IsAccFrom = 1
             THEN

              SELECT CASE                                            WHEN at.AccDmType = 0                                              THEN                                              1                                            ELSE                                            at.CanDebit                                    END, at.IsLimited, at.LimitedVal INTO v_CanDepit, v_IsLimted, v_LimtedVal FROM
                      AccountsTb at
              WHERE
                      at.AccID = p_AccIDFrom;

              IF IFNULL(Account_GetAccVal(p_AccIDFrom, p_RecievedCurrencyID, 0), 0) < v_NetTotla
                AND
                v_CanDepit = 0
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا رصيد الحساب غير كافي لإجراء هذه العملية';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_CanDepit = 1
                AND
                v_IsLimted = 1
                AND
                (v_LimtedVal + IFNULL(Account_GetAccVal(p_AccIDFrom, p_RecievedCurrencyID, 0), 0)) < v_NetTotla
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا القيمة المراد تحويلها أكبر من الحد المسموح به';
                  ROLLBACK;
                  LEAVE proc;
                END IF;
            END IF;




          SELECT at.CanDebit, at.IsLimited, at.LimitedVal INTO v_CanDepit, v_IsLimted, v_LimtedVal FROM
                  AccountsTb at
          WHERE
                  at.AccID = v_CurrentAccID;

          IF v_RBrType = 3
             THEN
              IF IFNULL(Account_GetAccVal(v_CurrentAccID, p_RecievedCurrencyID, 0), 0) < v_NetTotla
                AND
                v_CanDepit = 0
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا رصيد الوكيل غير كافي لإجراء هذه العملية';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF
                v_CanDepit = 1
                AND
                v_IsLimted = 1
                AND
                (v_LimtedVal + IFNULL(Account_GetAccVal(v_CurrentAccID, p_RecievedCurrencyID, 0), 0)) < v_NetTotla
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا القيمة المراد تحويلها أكبر من الحد المسموح به للوكيل';
                  ROLLBACK;
                  LEAVE proc;
                END IF;
            END IF;

          IF v_RBrType <> 3
            AND
            p_BranchRecievedID <> v_MainBr
             THEN


              SELECT a.IsLimited, IFNULL(a.LimitedVal, 0) INTO v_IsBranchLimited, v_BranchLimitedVal FROM
                      AccountsTb a
              WHERE
                      a.AccID = v_CurrentAccID;


              IF v_IsBranchLimited = 1
                 THEN

                  SET v_TotalAllowedLimit = IFNULL(v_BranchLimitedVal + Branch_GetAccVal(p_BranchRecievedID), 0);
                  IF v_NetTotla > v_TotalAllowedLimit
                     THEN
                      SET p_msgIN = 0;
                      SET p_MSG = N'عذراً، لقد تجاوز الفرع الحد المسموح به';
                      ROLLBACK;
                      LEAVE proc;
                    END IF;
                END IF;


            END IF;








          INSERT INTO `InternalEx`
                (
                  `Code`,
                  `SenderName`,
                  `SPhone1`,
                  `SPhone2`,
                  `SenderIDNo`,
                  `RecievedName`,
                  `RPhone1`,
                  `RPhone2`,
                  `RecievedIDNo`,
                  `RecievedCurrencyID`,
                  `DeliveredCurrencyID`,
                  `OverallVal`,
                  `ExVal`,
                  `ConfirmType`,
                  `SafeRecievedID`,
                  `DeliveryPlace`,
                  `BranchRecievedID`,
                  `BranchDeliveredID`,
                  `Notes`,
                  IDCode,
                  IsAccFrom,
                  AccFrom,
                  IsAccTo,
                  AccTo,
                  EXTRVAL,
                  BBranchAccID,
                  ServiceType
                )
          VALUES
                  (
                    p_Code, p_SenderName, p_SPhone1, p_SPhone2, p_SenderIDNo, p_RecievedName, p_RPhone1, p_RPhone2, p_RecievedIDNo, p_RecievedCurrencyID, p_DeliveredCurrencyID, p_OverallVal, p_ExVal, 0, p_SafeID, p_DeliveryPlace, p_BranchRecievedID, p_BranchDeliveredID, p_Notes, p_IDCode, p_IsAccFrom, p_AccIDFrom, p_IsAccTo, p_AccIDTo, p_EXTRVAL, p_BBRANCHID, p_ServiceType
                  );




          IF v_RBrType <> 3
             THEN


              IF p_IsAccFrom <> 1
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, v_NetTotla + p_EXTRVAL, 0, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_SafeOrAcc, v_AccFrom, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );

                END IF;



              IF p_IsAccFrom = 1
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, p_OverallVal, 0, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_SafeOrAcc, v_AccFrom, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );

                  IF p_ExVal > 0
                     THEN
                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                      VALUES
                              (
                                p_SafeID, p_ExVal, 0, NOW(), p_Code, 94, 94, p_BranchRecievedID, v_SafeOrAcc, v_AccIncome, ' عمولة تحويل', p_RecievedCurrencyID, p_Notes, v_MovementType
                              );
                    END IF;

                END IF;






              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, v_NetTotla, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_AccFrom, v_SafeOrAcc, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );


              IF p_EXTRVAL > 0
                AND
                p_IsAccFrom = 2
                 THEN
                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, p_EXTRVAL, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_BBRANCHALLOWACCID, v_AccFrom, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;
            END IF;


        END IF;

        END IF;   -- close the ConfirmType = 0 (save) block

      -- GOTO Confirm jumps INTO this block, landing at the Confirm label below and skipping the
      -- preamble. So it is THIS guard that must also accept the flag - not the save guard above.
      IF (p_ConfirmType = 1 OR v_goto_Confirm = 1)
         THEN
          IF v_goto_Confirm = 0 THEN   -- preamble skipped when jumping to Confirm



          SELECT ConfirmType INTO v_ID FROM
                  InternalEx
          WHERE
                  Code = p_Code;
          IF v_ID = 1
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'عذرا تم إعتماد هذه الحوالة مسبقا';
              ROLLBACK;
              LEAVE proc;
            END IF;

          IF p_ExVal >= p_OverallVal
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
              ROLLBACK;
              LEAVE proc;
            END IF;

          IF p_BranchRecievedID = p_BranchDeliveredID
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'عذرا يجب اختيار وجهة مختلفة عن الوجهة المستلمة';
              ROLLBACK;
              LEAVE proc;
            END IF;






          IF p_IsAccFrom = 0
            AND
            v_MobileType <> 1
            AND
            v_ID=0
             THEN
              UPDATE
                      ExSyAccounts_AccSafeActivityTb
              SET
                      Credit = v_NetTotla
              WHERE
                Credit <> 0
                AND ISID = p_Code;

              UPDATE
                      ExSyAccounts_AccSafeActivityTb
              SET
                      Debit = v_NetTotla
              WHERE
                Debit <> 0
                AND ISID = p_Code;

              UPDATE
                      InternalEx
              SET
                      OverallVal = p_OverallVal, ExVal = p_ExVal, ConfirmedSafeID = p_SafeID
              WHERE
                Code = p_Code;
            END IF;
          END IF;   -- end skip-to-Confirm
          -- (label Confirm removed; reached via v_goto_Confirm)
          SET v_MovementType = CONCAT('مرسلة إلى/ ', p_RecievedName, ' -هـ- ', p_RPhone1);
          IF p_IsAccTo <> 1
             THEN
              UPDATE
                      InternalEx
              SET
                      BranchDeliveredID = p_BranchDeliveredID
              WHERE
                Code = p_Code;
            END IF;

          IF v_DBrType <> 3 AND p_DeliveryPlace>0
             THEN
              UPDATE
                      InternalEx
              SET
                      DeliveryPlace = p_DeliveryPlace
              WHERE
                Code = p_Code;
            END IF;



          IF p_BranchDeliveredID = v_MainBr
            AND
            v_RBrType = 3
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, p_OverallVal, NOW(), p_Code, 1, 3, p_BranchDeliveredID, v_AccAgentForDel, v_CurrentAccID, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );

              IF v_DBShare > 0
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_DBShare, NOW(), p_Code, 1, 5, p_BranchDeliveredID, v_AccOutcome, v_AccAgentForDel, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;

            END IF;

          IF (v_DBrType <> 3
            AND
            v_RBrType <> 3)
            OR
            (p_BranchDeliveredID <> v_MainBr
            AND
            v_RBrType = 3
            AND
            v_DBrType <> 3)
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, p_OverallVal, 0, NOW(), p_Code, 1, 3, p_BranchDeliveredID, v_AccTo, v_AccForDel, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );


              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, p_OverallVal, NOW(), p_Code, 1, 3, p_BranchDeliveredID, v_AccForDel, v_AccTo, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;


          IF p_BranchDeliveredID <> v_MainBr
            AND
            v_RBrType = 3
            AND
            v_DBrType <> 3
             THEN
              IF v_MainShare > 0
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_MainShare, NOW(), p_Code, 1, 6, v_MainBr, v_AccMainFromBranches, v_CurrentAccID, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;


              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, v_DBCurrnetVal, NOW(), p_Code, 1, 3, v_MainBr, v_BRRECDES, v_CurrentAccID, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;

          IF v_RBrType = 3
            AND
            v_DBrType <> 3


             THEN


              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, p_OverallVal, 0, NOW(), p_Code, 1, 4, v_MainBr, v_CurrentAccID, CASE WHEN p_BranchDeliveredID = v_MainBr THEN v_AccAgentForDel ELSE v_AccForDel END, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );

              IF p_ExVal - v_RBShare >= p_OverallVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF p_ExVal - v_RBShare > p_ExVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF p_ExVal - v_RBShare > 0
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, p_ExVal - v_RBShare, 0, NOW(), p_Code, 1, 94, v_MainBr, v_CurrentAccID, CASE WHEN p_BranchDeliveredID = v_MainBr THEN v_AccAgentForDel ELSE v_AccForDel END, 'عمولة حوالة داخلية', p_RecievedCurrencyID, p_Notes, 'عمولة حوالة داخلية'
                          );
                END IF;
            END IF;
          IF v_ID = 0
             THEN
              UPDATE
                      InternalEx
              SET
                      ConfirmType = 1, ConfirmedSafeID = p_SafeID, ConfirmDate = NOW()
              WHERE
                Code = p_Code;

              IF v_DBrType = 3
                OR
                p_IsAccTo = 1
                 THEN
                  SET v_goto_Delivered = 1;   -- jump replaced by flag
                END IF;
            END IF;
        END IF;

      -- GOTO Delivered fires from inside the ConfirmType = 1 block, so this gate must accept the
      -- flag as well; otherwise the jump can never enter the delivery block and the delivery
      -- ledger rows are silently never written.
      IF (p_ConfirmType = 2 OR v_goto_Delivered = 1)
         THEN


          IF v_goto_Delivered = 0 THEN   -- preamble skipped when jumping to Delivered
          IF p_IsAccTo = 0
             THEN

              IF IFNULL(Account_GetAccVal(v_AccUserID, p_RecievedCurrencyID, 0), 0) < p_OverallVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'رصيد الخزنة غير كاف لإجراء هذه العملية';
                  ROLLBACK;
                  LEAVE proc;
                END IF;
            END IF;


          SELECT ConfirmType INTO v_ID FROM
                  InternalEx
          WHERE
                  Code = p_Code;
          IF v_ID = 2
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'عذرا تم تسليم هذه الحوالة مسبقا';
              ROLLBACK;
              LEAVE proc;
            END IF;
          END IF;   -- end skip-to-Delivered (preamble)













          IF (p_IsAccTo = 0 OR v_goto_Delivered = 1)
             THEN
          IF v_goto_Delivered = 0 THEN   -- skipped when jumping to Delivered


              IF NOT EXISTS (SELECT
                            1
                    FROM
                            TB_Users t
                    WHERE
                            t.BranchID = p_BranchDeliveredID
                            AND t.AccID = v_AccUserID)
                 THEN

                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا تم تغيير وجهة الحوالة ولايمكنك تسليمها حاليا';
                  ROLLBACK;
                  LEAVE proc;

                END IF;


          END IF;   -- end skip-to-Delivered
              -- (label Delivered removed; reached via v_goto_Delivered)
              SET v_SafeOrAcc = v_AccUserID;
            END IF;
          IF p_IsAccTo = 1
             THEN
              SET v_SafeOrAcc = p_AccIDTo;
            END IF;




          IF p_ExVal >= p_OverallVal
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'عذرا حدث خطأ أثناء الاعتماد الرجاء المحاولة مرة أخرى';
              ROLLBACK;
              LEAVE proc;
            END IF;

          IF p_ISHandallEX = 1
             THEN
              IF v_DBrType = 3
                AND
                v_RBrType <> 3
                AND
                p_BranchRecievedID = v_MainBr
                 THEN
                  SET v_RBShare = (p_ExVal - p_HandallExVal);
                  SET v_DBShare = p_HandallExVal;
                  SET v_MainShare = (p_ExVal - p_HandallExVal);
                            UPDATE
                  InternalEx
          SET
                BdShare=p_HandallExVal
          WHERE
            Code = p_Code;
                END IF;
              IF v_DBrType = 3
                AND
                v_RBrType <> 3
                AND
                p_BranchRecievedID <> v_MainBr
                 THEN
                  SET v_RBShare = (p_ExVal - p_HandallExVal) / 2;
                  SET v_DBShare = p_HandallExVal;
                  SET v_MainShare = (p_ExVal - p_HandallExVal) / 2;
                            UPDATE
                  InternalEx
          SET
                BdShare=p_HandallExVal
          WHERE
            Code = p_Code;
                END IF;

              IF v_DBrType = 3
                AND
                v_RBrType = 3
                 THEN
                  SET v_RBShare = p_HandallExVal;
                  SET v_DBShare = p_HandallExVal2;
                  SET v_MainShare = (p_ExVal - p_HandallExVal - p_HandallExVal2);
                            UPDATE
                  InternalEx
          SET
          BrShare=p_HandallExVal, BdShare=p_HandallExVal2
          WHERE
            Code = p_Code;
                END IF;
            END IF;

          SET v_RBCurrnetVal = IFNULL(v_NetTotla - v_RBShare, 0);
          SET v_DBCurrnetVal = p_OverallVal + v_DBShare;
          IF p_BranchDeliveredID = v_MainBr
            AND
            v_RBrType = 3
             THEN
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, p_OverallVal, 0, NOW(), p_Code, 1, 5, p_BranchDeliveredID, v_AccAgentForDel, v_SafeOrAcc, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );

            END IF;


          IF v_RBrType <> 3
             THEN



              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, v_NetTotla, 0, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_AccFrom, CASE WHEN p_BranchRecievedID = v_MainBr OR p_BranchDeliveredID = v_MainBr THEN v_CurrentDelAccID ELSE v_MainAcccurrent END, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );



              IF v_RBShare >= p_OverallVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_RBShare > p_ExVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_RBShare > 0
                 THEN
                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_RBShare, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_AccIncome, v_AccFrom, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;
            END IF;


          IF v_DBrType <> 3
             THEN
              SET v_MovementType = CONCAT('مسلمة إلى/ ', p_RecievedName);
              IF p_IsAccTo = 1
                 THEN
                  SET v_MovementType = CONCAT('محولة إلى حساب/ ', p_RecievedName);
                END IF;
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, p_OverallVal, NOW(), p_Code, 1, 5, p_BranchDeliveredID, v_SafeOrAcc, CASE WHEN v_RBrType = 3 AND p_BranchDeliveredID = v_MainBr THEN v_AccAgentForDel ELSE v_AccForDel END, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;



          IF (v_DBrType <> 3
            AND
            v_RBrType <> 3)
            OR
            (p_BranchDeliveredID <> v_MainBr
            AND
            v_RBrType = 3
            AND
            v_DBrType <> 3)
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, p_OverallVal, NOW(), p_Code, 1, 5, p_BranchDeliveredID, v_AccTo, CASE WHEN p_BranchRecievedID = v_MainBr OR p_BranchDeliveredID = v_MainBr THEN v_CurrentAccID ELSE v_MainAcccurrent END, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );


              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, p_OverallVal, 0, NOW(), p_Code, 1, 5, p_BranchDeliveredID, v_AccForDel, v_SafeOrAcc, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );


              IF v_DBShare > 0
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_DBShare, NOW(), p_Code, 1, 5, p_BranchDeliveredID, v_AccOutcome, v_AccTo, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;
            END IF;


          IF (p_BranchRecievedID <> v_MainBr
            AND
            v_RBrType <> 3)
            OR
            (v_RBrType = 3
            AND
            v_DBrType = 3)
             THEN
              IF v_MainShare > 0
                 THEN
                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_MainShare, NOW(), p_Code, 1, 6, v_MainBr, v_AccMainFromBranches, v_CurrentAccID, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;
            END IF;


          IF (p_BranchRecievedID = v_MainBr
            AND
            v_DBrType <> 3
            AND
            p_BranchDeliveredID <> v_MainBr)
            OR
            (p_BranchRecievedID <> v_MainBr
            AND
            v_RBrType <> 3)
            OR
            (v_DBrType = 3
            AND
            v_RBrType = 3)






             THEN

              IF v_RBrType = 3
                 THEN
                  SET v_MovementType = CONCAT('مرسلة إلى/ ', p_RecievedName, ' -هـ- ', p_RPhone1);
                END IF;

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, p_OverallVal, 0, NOW(), p_Code, 1, 4, CASE WHEN (p_BranchRecievedID = v_MainBr) THEN p_BranchDeliveredID ELSE v_MainBr END, v_CurrentAccID, CASE WHEN p_BranchRecievedID = v_MainBr OR p_BranchDeliveredID = v_MainBr THEN v_AccTo ELSE v_CurrentDelAccID END, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
              IF p_ExVal - v_RBShare > 0
                 THEN
                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, p_ExVal - v_RBShare, 0, NOW(), p_Code, 1, 94, CASE WHEN (p_BranchRecievedID = v_MainBr) THEN p_BranchDeliveredID ELSE v_MainBr END, v_CurrentAccID, CASE WHEN p_BranchRecievedID = v_MainBr OR p_BranchDeliveredID = v_MainBr THEN v_AccTo ELSE v_CurrentDelAccID END, 'عمولة حوالة داخلية', p_RecievedCurrencyID, p_Notes, 'عمولة حوالة داخلية'
                          );
                END IF;
            END IF;

          IF (p_BranchDeliveredID <> v_MainBr)
            OR
            (p_BranchDeliveredID = v_MainBr
            AND
            p_BranchRecievedID <> v_MainBr
            AND
            v_RBrType <> 3)
             THEN

              IF v_DBrType = 3
                 THEN
                  SET v_MovementType = CONCAT('مسلمة إلى/ ', p_RecievedName, ' -هـ- ', p_RPhone1);
                END IF;

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, p_OverallVal, NOW(), p_Code, 1, 4, CASE WHEN p_BranchDeliveredID = v_MainBr THEN p_BranchRecievedID ELSE v_MainBr END, v_CurrentDelAccID, CASE WHEN p_BranchDeliveredID = v_MainBr OR p_BranchRecievedID = v_MainBr THEN v_AccFrom ELSE v_CurrentAccID END, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );


              IF v_DBShare >= p_OverallVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_DBShare > p_ExVal
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا حدث خلل أثناء الإعتماد الرجاء المحاولة مرة أخرى';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_DBShare > 0
                 THEN
                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_DBShare, NOW(), p_Code, 1, 94, CASE WHEN p_BranchDeliveredID = v_MainBr THEN p_BranchRecievedID ELSE v_MainBr END, v_CurrentDelAccID, CASE WHEN p_BranchDeliveredID = v_MainBr OR p_BranchRecievedID = v_MainBr THEN v_AccFrom ELSE v_CurrentAccID END, 'عمولة حوالة داخلية', p_RecievedCurrencyID, p_Notes, 'عمولة حوالة داخلية'
                          );
                END IF;
            END IF;


          IF v_RBrType <> 3
            AND
            p_BranchRecievedID <> v_MainBr
            AND
            p_BranchDeliveredID <> v_MainBr
             THEN
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, v_RBCurrnetVal, NOW(), p_Code, 1, 4, p_BranchRecievedID, v_MainAcccurrent, v_CurrentDelAccID, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;


          IF v_DBrType <> 3
            AND
            p_BranchRecievedID <> v_MainBr
            AND
            p_BranchDeliveredID <> v_MainBr
             THEN
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, v_DBCurrnetVal, 0, NOW(), p_Code, 1, 6, p_BranchDeliveredID, v_MainAcccurrent, v_CurrentAccID, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;


          IF p_BranchDeliveredID <> v_MainBr
            AND
            v_RBrType = 3
            AND
            v_DBrType <> 3
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, v_DBCurrnetVal, 0, NOW(), p_Code, 1, 6, v_MainBr, v_BRRECDES, v_MainAcccurrent, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );
            END IF;




SELECT SUM(Debit), SUM(Credit) INTO v_SumDebit, v_SumCredit FROM ExSyAccounts_AccSafeActivityTb
WHERE ISID = p_Code;

IF IFNULL(v_SumDebit, 0) <> IFNULL(v_SumCredit, 0)
 THEN
    SET p_msgIN = 0;
    SET p_MSG = N'حدث خطأ أثناء الاعتماد الرجاء المحاولة مرة اخرى';
    ROLLBACK;
    LEAVE proc;
END IF;



          UPDATE
                  InternalEx
          SET
                  ConfirmType = 2, SafeDeliveredID = p_SafeID, RecievedDate = NOW()
          WHERE
            Code = p_Code;
        END IF;

      IF p_ConfirmType = 10
         THEN


          SELECT a.ConfirmType, IFNULL(a.IsCanceled, 0) INTO v_OldConfType, v_IsCanceled FROM
                  InternalEx a
          WHERE
                  a.Code = p_Code;




          IF v_OldConfType = 0
             THEN
              SET v_OldConfType = 3;
              SET v_goto_CancleConfirm = 1;   -- jump replaced by flag
            END IF;


          IF v_OldConfType = 4
            OR
            v_OldConfType = 3
             THEN
              UPDATE
                      InternalEx
              SET
                      IsCanceled = 0, ConfirmType = v_OldConfType - 3
              WHERE
                Code = p_Code;
            END IF;
        END IF;

      IF (p_ConfirmType = 5 OR v_goto_CancleConfirm = 1)
         THEN
          IF v_goto_CancleConfirm = 0 THEN   -- skipped when jumping to CancleConfirm



          SELECT a.ConfirmType, IFNULL(a.IsCanceled, 0) INTO v_OldConfType, v_IsCanceled FROM
                  InternalEx a
          WHERE
                  a.Code = p_Code;

          IF p_IsAccFrom = 0
             THEN
              SET v_SafeOrAcc = v_AccUserID;
            END IF;
          IF p_IsAccFrom <> 0
             THEN
              SET v_SafeOrAcc = v_AccFrom;
            END IF;

          END IF;   -- end skip-to-CancleConfirm
          -- (label CancleConfirm removed; reached via v_goto_CancleConfirm)
          SET v_MovementType = 'حوالة داخلية ملغاة';
          IF v_OldConfType = 3
             THEN
              IF v_RBrType <> 3
                 THEN

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, v_NetTotla, NOW(), p_Code, 1, 25, p_BranchRecievedID, v_AccCancelRB, v_AccFrom, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                  IF p_IsAccFrom <> 0
                     THEN

                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                          SELECT
                                  p_SafeID,
                                  a.Credit,
                                  a.Debit,
                                  NOW(),
                                  a.ISID,
                                  1,
                                  25,
                                  a.AccBranchID,
                                  a.AccIDFrom,
                                  v_AccCancelRB,
                                  v_MovementType,
                                  p_RecievedCurrencyID,
                                  p_Notes,
                                  v_MovementType
                          FROM
                                  ExSyAccounts_AccSafeActivityTb a
                          WHERE
                                  a.ISID = p_Code
                          ORDER BY
                                  a.ID;
                    END IF;
                  IF p_IsAccFrom = 0
                     THEN

                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                      VALUES
                              (
                                p_SafeID, v_NetTotla, 0, NOW(), p_Code, 1, 25, p_BranchRecievedID, v_AccFrom, v_AccCancelRB, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                              );
                    END IF;
                END IF;
              UPDATE
                      InternalEx
              SET
                      ConfirmType = CASE                                             WHEN p_IsAccFrom = 0                                               AND v_RBrType <> 3                                               THEN                                               5                                             ELSE                                             6                                     END, ConfirmCanceledSafeID = p_SafeID, ConfirmCanceledDate = NOW()
              WHERE
                Code = p_Code;
            END IF;

          IF v_OldConfType = 4
             THEN
              IF v_DBrType = 3
                OR
                p_BranchRecievedID = p_BranchDeliveredID
                 THEN
                  SET v_IsCanceled = 1;
                END IF;
              IF v_IsCanceled = 1
                 THEN

                  IF v_RBrType <> 3
                     THEN

                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                      VALUES
                              (
                                p_SafeID, 0, v_NetTotla, NOW(), p_Code, 1, 25, p_BranchRecievedID, v_AccCancelRB, v_AccFrom, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                              );

                      IF v_DBrType <> 3
                         THEN

                          INSERT INTO ExSyAccounts_AccSafeActivityTb
                                (
                                  SafeID,
                                  Debit,
                                  Credit,
                                  InsertDate,
                                  ISID,
                                  TypeID,
                                  OperationTypeID,
                                  AccBranchID,
                                  AccIDFrom,
                                  AccIDTo,
                                  MovementType,
                                  CurrencyID,
                                  Note,
                                  SafeIDMovement
                                )
                          VALUES
                                  (
                                    p_SafeID, v_NetTotla, 0, NOW(), p_Code, 1, 25, p_BranchRecievedID, v_AccFrom, v_AccCancelRB, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                                  );
                        END IF;
                    END IF;

                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                      SELECT
                              p_SafeID,
                              a.Credit,
                              a.Debit,
                              NOW(),
                              a.ISID,
                              1,
                              25,
                              a.AccBranchID,
                              a.AccIDFrom,
                              CASE                                        WHEN p_BranchDeliveredID = AccBranchID                                          OR v_RBrType = 3                                          THEN                                          a.AccIDTo                                        ELSE                                        v_AccCancelRB                                END,
                              v_MovementType,
                              p_RecievedCurrencyID,
                              p_Notes,
                              v_MovementType
                      FROM
                              ExSyAccounts_AccSafeActivityTb a
                      WHERE
                              a.ISID = p_Code
                              AND (   a.OperationTypeID = 4
                                      OR a.OperationTypeID = 3

                                      OR a.OperationTypeID = 94)
                      ORDER BY
                              a.ID;


                  IF EXISTS (SELECT
                                1
                        FROM
                                ExSyAccounts_AccSafeActivityTb a
                        WHERE
                                a.ISID = p_Code
                                AND a.AccIDFrom = v_AccIncome
                                AND a.Debit = 0)
                     THEN
                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                          SELECT p_SafeID,
                                  a.Credit,
                                  a.Debit,
                                  NOW(),
                                  a.ISID,
                                  1,
                                  25,
                                  a.AccBranchID,
                                  v_AccCancleIncome,
                                  v_AccCancelRB,
                                  v_MovementType,
                                  p_RecievedCurrencyID,
                                  p_Notes,
                                  v_MovementType
                          FROM
                                  ExSyAccounts_AccSafeActivityTb a
                          WHERE
                                  a.ISID = p_Code
                                  AND a.AccIDFrom = v_AccIncome
                                  AND a.Debit = 0
                          ORDER BY
                                  a.ID DESC LIMIT 1 ;
                    END IF;


                  IF EXISTS (SELECT
                                1
                        FROM
                                ExSyAccounts_AccSafeActivityTb a
                        WHERE
                                a.ISID = p_Code
                                AND a.AccIDFrom = v_AccOutcome
                                AND a.Debit = 0)
                     THEN
                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                          SELECT p_SafeID,
                                  a.Credit,
                                  a.Debit,
                                  NOW(),
                                  a.ISID,
                                  1,
                                  25,
                                  a.AccBranchID,
                                  v_AccLossOutcome,
                                  v_AccAgentForDel,
                                  v_MovementType,
                                  p_RecievedCurrencyID,
                                  p_Notes,
                                  v_MovementType
                          FROM
                                  ExSyAccounts_AccSafeActivityTb a
                          WHERE
                                  a.ISID = p_Code
                                  AND a.AccIDFrom = v_AccOutcome
                                  AND a.Debit = 0
                          ORDER BY
                                  a.ID DESC LIMIT 1 ;
                    END IF;


                  IF EXISTS (SELECT
                                1
                        FROM
                                ExSyAccounts_AccSafeActivityTb a
                        WHERE
                                a.ISID = p_Code
                                AND a.AccIDFrom = v_AccMainFromBranches
                                AND a.Debit = 0)
                    AND
                    p_BranchDeliveredID <> v_MainBr
                     THEN
                      INSERT INTO ExSyAccounts_AccSafeActivityTb
                            (
                              SafeID,
                              Debit,
                              Credit,
                              InsertDate,
                              ISID,
                              TypeID,
                              OperationTypeID,
                              AccBranchID,
                              AccIDFrom,
                              AccIDTo,
                              MovementType,
                              CurrencyID,
                              Note,
                              SafeIDMovement
                            )
                          SELECT p_SafeID,
                                  a.Credit,
                                  a.Debit,
                                  NOW(),
                                  a.ISID,
                                  1,
                                  25,
                                  a.AccBranchID,
                                  v_AccLossMainFromBranches,
                                  v_CurrentAccID,
                                  v_MovementType,
                                  p_RecievedCurrencyID,
                                  p_Notes,
                                  v_MovementType
                          FROM
                                  ExSyAccounts_AccSafeActivityTb a
                          WHERE
                                  a.ISID = p_Code
                                  AND a.AccIDFrom = v_AccMainFromBranches
                                  AND a.Debit = 0
                          ORDER BY
                                  a.ID DESC LIMIT 1 ;
                    END IF;
                  UPDATE
                          InternalEx
                  SET
                          ConfirmType = CASE                                                 WHEN v_RBrType <> 3                                                   THEN                                                   5                                                 ELSE                                                 6                                         END
                  WHERE
                    Code = p_Code;
                END IF;
            END IF;

          UPDATE
                  InternalEx
          SET
                  IsCanceled = 1, ConfirmCanceledSafeID = p_SafeID, ConfirmCanceledDate = NOW()
          WHERE
            Code = p_Code;

          IF p_IsAccFrom = 1
            AND
            v_OldConfType = 4
            AND
            v_IsCanceled = 1
             THEN
              SET v_goto_DeliveredCancle = 1;   -- jump replaced by flag
            END IF;

        END IF;

      IF (p_ConfirmType = 6 OR v_goto_DeliveredCancle = 1)
         THEN
          -- (label DeliveredCancle removed; reached via v_goto_DeliveredCancle)

          IF p_IsAccFrom = 0
             THEN
              SET v_SafeOrAcc = v_AccUserID;
            END IF;
          IF p_IsAccFrom <> 0
             THEN
              SET v_SafeOrAcc = p_AccIDFrom;
            END IF;


          IF v_RBrType <> 3
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, v_NetTotla, 0, NOW(), p_Code, 1, 25, p_BranchRecievedID, v_AccCancelRB, v_SafeOrAcc, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );


              SET v_MovementType = 'حوالة ملغاة';
              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      MovementType,
                      CurrencyID,
                      Note,
                      SafeIDMovement
                    )
              VALUES
                      (
                        p_SafeID, 0, p_OverallVal, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_SafeOrAcc, v_AccCancelRB, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                      );

              IF p_IsAccFrom = 0
                 THEN

                  SET v_MovementType = 'حوالة ملغاة';
                  INSERT INTO ExSyAccounts_AccSafeActivityTb
                        (
                          SafeID,
                          Debit,
                          Credit,
                          InsertDate,
                          ISID,
                          TypeID,
                          OperationTypeID,
                          AccBranchID,
                          AccIDFrom,
                          AccIDTo,
                          MovementType,
                          CurrencyID,
                          Note,
                          SafeIDMovement
                        )
                  VALUES
                          (
                            p_SafeID, 0, p_ExVal, NOW(), p_Code, 1, 1, p_BranchRecievedID, v_SafeOrAcc, v_AccCancelRB, v_MovementType, p_RecievedCurrencyID, p_Notes, v_MovementType
                          );
                END IF;
            END IF;
          UPDATE
                  InternalEx
          SET
                  ConfirmType = 6, SafeDeliveredID = p_SafeID, RecievedDate = NOW()
          WHERE
            Code = p_Code;
        END IF;

      COMMIT;
      SET p_msgIN = 1;
      SET p_MSG = N'تم التنفيذ بنجاح';
  END
$$
DELIMITER ;
