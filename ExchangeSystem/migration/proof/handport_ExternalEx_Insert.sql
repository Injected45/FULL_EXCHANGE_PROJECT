-- =====================================================================================
-- Hand-port: ExternalEx_Insert  (T-SQL jump -> flag; MySQL has no such jump statement)
--
-- The source contains ONE forward jump, and it is genuinely unstructured: it leaves the
-- ConfirmedType=1 branch and lands in the MIDDLE of the ConfirmedType=2 branch.
--   jump site  : IF @ConfirmedType = 1 -> IF @IsInOrOut = 1 -> IF @IsAccTo = 1 -> jump IsHasAccTO
--   label site : IF @ConfirmedType = 2 -> IF @IsInOrOut = 1 -> IsHasAccTO:
--
-- Verified before translating (ledger code - equivalence checked, not assumed):
--   * 'IsHasAccTO' occurs EXACTLY twice in the source: the jump and the label. No other jumps.
--   * the jump is the LAST statement in its branch - only block ENDs follow, so nothing is skipped.
--   * BOTH sites sit under IsInOrOut = 1, so that guard is already true on arrival.
--   * between 'IF @ConfirmedType = 2' and the label there are ONLY comments - no statement that
--     would newly execute when arriving via the flag.
-- The only differing guard is therefore ConfirmedType, making the rewrite exact:
--     jump IsHasAccTO       ->  SET v_goto_IsHasAccTO = 1  (then fall out of the branch)
--     IF @ConfirmedType = 2 ->  IF (p_ConfirmedType = 2 OR v_goto_IsHasAccTO = 1)
--     IsHasAccTO:           ->  removed
-- Body is otherwise the converter's own output, unchanged.
--
-- !! NEEDS A LIVE TEST: perform an external transfer with IsAccTo=1 and confirm the resulting
-- !! AccSafeActivityTb rows match SQL Server. Control-flow changes in ledger code cannot be
-- !! proven by static checks alone.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ExternalEx_Insert`;
DELIMITER $$
CREATE PROCEDURE `ExternalEx_Insert`(IN `p_IDCode` BIGINT, IN `p_Code` LONGTEXT, IN `p_SenderName` VARCHAR(50), IN `p_SPhone1` VARCHAR(50), IN `p_SPhone2` VARCHAR(50), IN `p_SenderIDNo` VARCHAR(50), IN `p_RecievedCurrencyID` INT, IN `p_CountryIDFrom` INT, IN `p_BranchRecievedID` INT, IN `p_RecievedName` VARCHAR(50), IN `p_RPhone1` VARCHAR(50), IN `p_RPhone2` VARCHAR(50), IN `p_CityIDTo` VARCHAR(50), IN `p_DeliveredCurrencyID` INT, IN `p_CountryIDTo` INT, IN `p_AgentCheck` INT, IN `p_IsPrivateAccount` TINYINT(1), IN `p_OwnNatioNum` VARCHAR(50), IN `p_OwnAccNo` VARCHAR(50), IN `p_ServiceType` INT, IN `p_IsServiceVal` TINYINT(1), IN `p_ServiceExVal` DECIMAL(12,3), IN `p_BranchDeliveredID` INT, IN `p_CurrRecievedVal` DECIMAL(18,3), IN `p_ExVal` DECIMAL(12,3), IN `p_ExtraVal` DECIMAL(12,3), IN `p_OverallVal` DECIMAL(18,3), IN `p_InsertDate` DATE, IN `p_SafeRecievedID` INT, IN `p_SafeDeliveredID` INT, IN `p_RecievedDate` DATE, IN `p_IsDelivered` TINYINT(1), IN `p_Notes` LONGTEXT, IN `p_IsAccFrom` TINYINT UNSIGNED, IN `p_AccFID` BIGINT, IN `p_IsAccTo` TINYINT UNSIGNED, IN `p_TransAccIDTo` BIGINT, IN `p_IsCash` TINYINT UNSIGNED, IN `p_TransPrice` DECIMAL(12,3), IN `p_BBranchAccID` BIGINT, IN `p_BBRANCHID` INT, IN `p_EXTRVAL` DECIMAL(18,3), IN `p_BankServiceType` INT, INOUT `p_msgIN` INT, IN `p_Description` LONGTEXT, IN `p_BranchRecievedName` LONGTEXT, INOUT `p_MSG` LONGTEXT, IN `p_EMPCUSTSELECT` INT, IN `p_EMPTOSELECT` INT, IN `p_IsInOrOut` TINYINT UNSIGNED, IN `p_ConfirmedType` INT, IN `p_NewTrancPrice` DECIMAL(12,6), IN `p_NewFinalTotal` DECIMAL(12,3), IN `p_OurAccID` BIGINT, IN `p_ConfirmedSafeID` INT, IN `p_ConfirmDate` DATE, IN `p_CurrDeliveredVal` DECIMAL(12,3), IN `p_IsHandelExAVal` TINYINT(1), IN `p_HandelExAVal` DECIMAL(12,3), IN `p_BankIDTo` INT, IN `p_TransPrice1` DECIMAL(12,3), INOUT `p_NewPrice_SenDForWhatsapp` INT)
proc: BEGIN
DECLARE v_goto_IsHasAccTO INT DEFAULT 0;
DECLARE v_ID BIGINT;
DECLARE v_RBShare           DECIMAL(12, 3);
DECLARE v_DBShare           DECIMAL(12, 3);
DECLARE v_AgnetShare DECIMAL(12, 3);
DECLARE v_NETTotal DECIMAL(12, 3);
DECLARE v_MainShare DECIMAL(12, 3);
DECLARE v_BRRECDES BIGINT;
DECLARE v_BRMAINDES BIGINT;
DECLARE v_BBRANCHALLOWACCID BIGINT;
DECLARE v_CostPrice DECIMAL(12, 3);
DECLARE v_OurBalance DECIMAL(12, 3);
DECLARE v_BounsSaleVal DECIMAL(12, 3);
DECLARE v_MovementType LONGTEXT;
DECLARE v_AccToMizan BIGINT;
DECLARE v_IsBranchLimited BIT;
DECLARE v_BranchLimitedVal DECIMAL(18, 3);
DECLARE v_TotalAllowedLimit DECIMAL(18, 3);
DECLARE v_MovementUserType LONGTEXT;
DECLARE v_MovementUserTypeTo LONGTEXT;
DECLARE v_RBrType INT;
DECLARE v_DBrType INT;
DECLARE v_AccFrom BIGINT;
DECLARE v_AccCancleFrom BIGINT;
DECLARE v_AccUserID BIGINT;
DECLARE v_AccTo BIGINT;
DECLARE v_AccForDel BIGINT;
DECLARE v_MainBr INT;
DECLARE v_AccIncome BIGINT;
DECLARE v_AccOutRcome BIGINT;
DECLARE v_CurrentDelAccID BIGINT;
DECLARE v_CurrentAccID BIGINT;
DECLARE v_AccMainFromBranches BIGINT;
DECLARE v_AccMainLoseFromBranches BIGINT;
DECLARE v_NetTot DECIMAL(12, 3);
DECLARE v_MainAcccurrent BIGINT;
DECLARE v_AccOutcome BIGINT;
DECLARE v_AccBounsSale BIGINT;
DECLARE v_AccloseSale BIGINT;
DECLARE v_AccRevenues BIGINT;
DECLARE v_AccLosses BIGINT;
DECLARE v_AccRevenues1 BIGINT;
DECLARE v_AccLosses1 BIGINT;
DECLARE v_FIRSTBRRATE DECIMAL(12, 3);
DECLARE v_SECOBRRATE DECIMAL(12, 3);
DECLARE v_RRateWithMain DECIMAL(12, 3);
DECLARE v_DRateWithMain DECIMAL(12, 3);
DECLARE v_DBRNAME LONGTEXT;
DECLARE v_AgentMovement LONGTEXT;
DECLARE v_BRRECNAME LONGTEXT;
DECLARE v_EMPCUSTACC BIGINT;
DECLARE v_OPETYPE INT;
DECLARE v_TPID INT;
DECLARE v_CUSTEMPID INT;
DECLARE v_CECODE LONGTEXT;
DECLARE v_ECCODEID BIGINT;
DECLARE v_WDNOTES LONGTEXT;
DECLARE v_GetMovementUserType   LONGTEXT;
DECLARE v_EMPCUSTACCTO BIGINT;
DECLARE v_OPETYPETO INT;
DECLARE v_TPIDTO INT;
DECLARE v_CUSTEMPIDTO INT;
DECLARE v_CECODETO LONGTEXT;
DECLARE v_ECCODEIDTO BIGINT;
DECLARE v_WDNOTESTO LONGTEXT;
DECLARE v_GetMovementUserTypeTO LONGTEXT;
DECLARE v_DefaultCounID INT;
DECLARE v_RBS DECIMAL(12, 3);
DECLARE v_RestVal DECIMAL(12, 3);
DECLARE v_OldTotalPrice DECIMAL(12, 3);
DECLARE v_OldTotalPrice1 DECIMAL(12, 3);
DECLARE v_NewPrice DECIMAL(12, 3);
DECLARE v_NewPrice1 DECIMAL(12, 3);
DECLARE v_CurrPower BIT;
DECLARE v_OldPrice DECIMAL(12, 3);
DECLARE v_OldPrice1 DECIMAL(12, 3);
DECLARE v_RevenuesOrLossesVal DECIMAL(12, 3);
DECLARE v_AccVal DECIMAL(12, 3);
DECLARE v_CanDEpit BIT;
DECLARE v_LimtedVal DECIMAL(12, 3);
DECLARE v_IsLimted BIT;
DECLARE v_Rate DECIMAL(12, 3);
DECLARE v_res DECIMAL(12, 3);
DECLARE v_BBRTOTALVAL DECIMAL(18, 3);
DECLARE v_CheckParm1 INT;
DECLARE v_CheckParm2 INT;
DECLARE v_IsConf INT;
DECLARE v_OldPrice2 DECIMAL(12, 3);
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_msgIN = 0; END;


      START TRANSACTION;




SET v_MovementUserType = CONCAT(p_BranchRecievedName, ' ', 'للمستلم', ' ', p_RecievedName);
      SELECT BranchType INTO v_RBrType FROM
              CoBranch
      WHERE
              ID = p_BranchRecievedID;
      SELECT BranchType INTO v_DBrType FROM
              CoBranch
      WHERE
              ID = p_BranchDeliveredID;

      SELECT AccID INTO v_AccFrom FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 2010603;


      SELECT AccID INTO v_AccCancleFrom FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 2010604;

      SELECT AccID INTO v_AccUserID FROM
              TB_Users
      WHERE
              USID = p_SafeRecievedID;


      SELECT a.AccID INTO v_BBRANCHALLOWACCID FROM
              AccountsTb AS a
      WHERE
              a.BranchID = p_BranchRecievedID
              AND a.AccParent = 40101018;

      SELECT AccID INTO v_AccTo FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 1010603;

      SELECT AccID INTO v_AccForDel FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 2010606;

      SELECT ID INTO v_MainBr FROM
              CoBranch
      WHERE
              IsMain = 1;

      SELECT AccID INTO v_AccIncome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 40101011;


      SELECT AccID INTO v_AccOutRcome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchRecievedID
              AND AccountsTb.AccParent = 30102010;

      SELECT CurrentAccID INTO v_CurrentDelAccID FROM
              CoBranch cb
      WHERE
              ID = p_BranchDeliveredID;

      SELECT CurrentAccID INTO v_CurrentAccID FROM
              CoBranch cb
      WHERE
              ID = p_BranchRecievedID;

      SELECT AccID INTO v_AccMainFromBranches FROM
              AccountsTb
      WHERE
              AccountsTb.AccCode = 4010101501;


      SELECT AccID INTO v_AccMainLoseFromBranches FROM
              AccountsTb
      WHERE
              AccountsTb.AccCode = 301020701
              AND BranchID = v_MainBr;

SET v_NetTot = p_CurrRecievedVal + p_ExVal;
      SELECT CurrentAccID INTO v_MainAcccurrent FROM
              CoBranch cb
      WHERE
              ID = v_MainBr;

      SELECT AccID INTO v_AccOutcome FROM
              AccountsTb
      WHERE
              AccountsTb.BranchID = p_BranchDeliveredID
              AND AccountsTb.AccParent = 40101012;

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


      SELECT at.AccID INTO v_AccBounsSale FROM
              AccountsTb at
      WHERE
              at.AccParent = 40101016
              AND at.BranchID = v_MainBr;


      SELECT at.AccID INTO v_AccloseSale FROM
              AccountsTb at
      WHERE
              at.AccParent = 30102011
              AND at.BranchID = v_MainBr;



      SELECT at.AccID INTO v_AccRevenues FROM
              AccountsTb at
      WHERE
              at.AccParent = 40101017
              AND at.BranchID = v_MainBr;



      SELECT at.AccID INTO v_AccLosses FROM
              AccountsTb at
      WHERE
              at.AccParent = 30102012
              AND at.BranchID = v_MainBr;



      SELECT at.AccID INTO v_AccRevenues1 FROM
              AccountsTb at
      WHERE
              at.AccParent = 40101017
              AND at.BranchID = p_BranchRecievedID;



      SELECT at.AccID INTO v_AccLosses1 FROM
              AccountsTb at
      WHERE
              at.AccParent = 30102012
              AND at.BranchID = p_BranchRecievedID;

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

      SELECT a.FBRate INTO v_RRateWithMain FROM
              BranchRatesTb AS a
      WHERE
              a.FBranchID = p_BranchRecievedID
              AND a.SBranchID = v_MainBr
      ORDER BY a.ID DESC LIMIT 1;

      SELECT a.SBRate INTO v_DRateWithMain FROM
              BranchRatesTb AS a
      WHERE
              a.SBranchID = p_BranchDeliveredID
              AND a.FBranchID = v_MainBr
      ORDER BY a.ID DESC LIMIT 1;
      SELECT BName INTO v_DBRNAME FROM
              CoBranch cb
      WHERE
              ID = p_BranchDeliveredID;
SET v_AgentMovement = CONCAT('حوالة صادرة', ' ', 'لـ/', p_RecievedName);
      SELECT cb.BName INTO v_BRRECNAME FROM
              CoBranch cb
      WHERE
              cb.ID = p_BranchRecievedID;
SET v_RBShare = p_ExVal / 100 * v_RRateWithMain;
SET v_RBS = p_CurrRecievedVal + v_RBShare;
SET v_RestVal = p_ExVal - v_RBShare;
      IF p_ConfirmedType = 0
         THEN
          SET p_msgIN = 1;

          SELECT IDCode INTO v_ID FROM
                  ExternalEx
          WHERE
                  IDCode = p_IDCode;

          IF v_ID = p_IDCode
             THEN
              SET p_msgIN = 0;
              SET p_MSG = 'عذرا هذا الرقم موجود مسبقاً الرجاء الحفظ مرة أخرى';
              ROLLBACK;
              LEAVE proc;
            END IF;


          IF p_IsAccFrom = 1
             THEN

              SELECT at.CanDebit, at.IsLimited, at.LimitedVal INTO v_CanDEpit, v_IsLimted, v_LimtedVal FROM
                      AccountsTb at
              WHERE
                      at.AccID = p_AccFID;

              IF IFNULL(Account_GetAccVal(p_AccFID, p_RecievedCurrencyID, 0), 0) < v_NetTot
                AND
                v_CanDEpit = 0
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا رصيد الحساب غير كافي لإجراء هذه العملية';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_CanDEpit = 1
                AND
                v_IsLimted = 1
                AND
                (v_LimtedVal + IFNULL(Account_GetAccVal(p_AccFID, p_RecievedCurrencyID, 0), 0)) < v_NetTot
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا القيمة المراد تحويلها أكبر من الحد المسموح به';
                  ROLLBACK;
                  LEAVE proc;
                END IF;
            END IF;




          SELECT at.CanDebit, at.IsLimited, at.LimitedVal INTO v_CanDEpit, v_IsLimted, v_LimtedVal FROM
                  AccountsTb at
          WHERE
                  at.AccID = v_CurrentAccID;

          IF v_RBrType = 3
             THEN
              IF IFNULL(Account_GetAccVal(v_CurrentAccID, p_RecievedCurrencyID, 0), 0) < v_NetTot
                AND
                v_CanDEpit = 0
                 THEN
                  SET p_msgIN = 0;
                  SET p_MSG = 'عذرا رصيد الوكيل غير كافي لإجراء هذه العملية';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF
                v_CanDEpit = 1
                AND
                v_IsLimted = 1
                AND
                (v_LimtedVal + IFNULL(Account_GetAccVal(v_CurrentAccID, p_RecievedCurrencyID, 0), 0)) < v_NetTot
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
                  IF v_NetTot > v_TotalAllowedLimit
                     THEN
                      SET p_msgIN = 0;
                      SET p_MSG = N'عذراً، لقد تجاوز الفرع الحد المسموح به';
                      ROLLBACK;
                      LEAVE proc;
                    END IF;
                END IF;


            END IF;


        END IF;



      IF p_ConfirmedType = 0
         THEN

          INSERT INTO ExternalEx
                (
                  IDcode,
                  Code,
                  SenderName,
                  Phone1,
                  Phone2,
                  SIDNo,
                  RecievedCurrencyID,
                  CountryIDFrom,
                  RecievedBranchID,
                  RecievedName,
                  RPhone1,
                  RPhone2,
                  CityIDTo,
                  DeliveredCurrencyID,
                  CountryIDTo,
                  TransType,
                  IsPrivateAccount,
                  OwnNatioNum,
                  OwnAccNo,
                  ServiceType,
                  IsServiceVal,
                  ServiceExVal,
                  BranchDeleviredID,
                  CurrRecievedVal,
                  ExVal,
                  ExtraVal,
                  NetTotal,
                  InsertDate,
                  SafeRecievedID,
                  SafeDeliveredID,
                  RecievedDate,
                  IsDelivered,
                  IsConfirmed,
                  ConfirmedSafeID,
                  Notes,
                  IsActive,
                  IsAccFrom,
                  RefuseCanceled,
                  AccFrom,
                  IsAccTo,
                  AccTo,
                  IsEdit,
                  IsCash,
                  TransPrice,
                  BBranchAccID,
                  BBRANCHID,
                  BankExVAL,
                  BankServiceType,
                  IsInOrOut,
                  CurrDeliveredVal,
                  BankIDTo,
                  InsertTime,
                  TransPrice1
                )
          VALUES
                  (
                    p_IDCode, p_Code, p_SenderName, p_SPhone1, p_SPhone2, p_SenderIDNo, p_RecievedCurrencyID, p_CountryIDFrom, p_BranchRecievedID, p_RecievedName, p_RPhone1, p_RPhone2, p_CityIDTo, p_DeliveredCurrencyID, p_CountryIDTo, p_AgentCheck, p_IsPrivateAccount, p_OwnNatioNum, p_OwnAccNo, p_ServiceType, p_IsServiceVal, p_ServiceExVal, p_BranchDeliveredID, p_CurrRecievedVal, p_ExVal, p_ExtraVal, p_OverallVal, NOW(), p_SafeRecievedID, p_SafeDeliveredID, p_RecievedDate, p_IsDelivered, 0, 0, p_Notes, 1, p_IsAccFrom, 0, p_AccFID, p_IsAccTo, p_TransAccIDTo, 0, p_IsCash, p_TransPrice, p_BBranchAccID, p_BBRANCHID, p_EXTRVAL, p_BankServiceType, p_IsInOrOut, p_CurrDeliveredVal, p_BankIDTo, NOW(), p_TransPrice1
                  );



          IF p_IsCash = 0
             THEN
              SET v_AccToMizan = v_AccUserID;
            END IF;

          IF p_IsCash = 1
             THEN
              SET v_AccToMizan = p_AccFID;
            END IF;

          IF p_IsCash = 2
             THEN
              SET v_AccToMizan = p_BBranchAccID;
            END IF;


          IF p_IsInOrOut = 0
             THEN

              IF (v_RBrType = 1
                OR
                v_RBrType = 2)
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NetTot, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccFrom, v_AccToMizan, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                END IF;



              IF (v_RBrType = 1
                OR
                v_RBrType = 2)
                AND
                (v_RBrType <> 3
                OR
                v_RBrType <> 4)

                 THEN

                  IF p_IsCash = 0
                     THEN

                      IF p_IsAccFrom = 1
                         THEN
                          SET v_AccToMizan = p_AccFID;
                        END IF;

                      IF p_IsAccFrom = 0
                         THEN
                          SET v_AccToMizan = v_AccFrom;
                        END IF;


                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NetTot, 0.000, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccUserID, v_AccFrom, 0, 0, p_BranchRecievedName, p_RecievedCurrencyID, 0, 0, v_MovementUserType);
                    END IF;

                  IF p_IsCash = 1
                    OR
                    p_IsCash = 0
                     THEN

                      IF p_IsAccFrom = 1
                        AND
                        p_IsAccTo = 0
                         THEN

                          IF p_EMPCUSTSELECT = 5
                             THEN
SET v_OPETYPE = 38;
SET v_TPID = 5;
                              SELECT a.ID INTO v_CUSTEMPID FROM
                                      EmployeeTb AS a
                              WHERE
                                      a.EMPNAME = p_SenderName;
                            END IF;

                          IF p_EMPCUSTSELECT = 6
                             THEN
SET v_OPETYPE = 39;
SET v_TPID = 6;
                              SELECT a.ID INTO v_CUSTEMPID FROM
                                      CustomersTb AS a
                              WHERE
                                      a.CustName = p_SenderName;
                            END IF;

                          IF p_EMPCUSTSELECT = 25
                             THEN
SET v_OPETYPE = 54;
SET v_TPID = 25;
                              SELECT a.ID INTO v_CUSTEMPID FROM
                                      CreditAccountsTb AS a
                              WHERE
                                      a.AccName = p_SenderName;
                            END IF;
SET v_MovementUserType = CONCAT(p_BranchRecievedName, ' ', 'للمستلم', ' ', p_RecievedName);
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_CurrRecievedVal, 0.000, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, p_AccFID, v_AccFrom, 0, 0, 'سحب بتحويل خارجي من حساب', p_RecievedCurrencyID, 0, 0, 'سحب بتحويل خارجي من حساب');
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_ExVal, 0.000, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, p_AccFID, v_AccFrom, 0, 0, 'عمولة تحويل', p_RecievedCurrencyID, 0, 0, 'عمولة تحويل');
                          IF p_IsCash = 0
                             THEN



                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NetTot, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccUserID, v_AccToMizan, 0, 0, p_BranchRecievedName, p_RecievedCurrencyID, 0, 0, v_MovementUserType);
                            END IF;

                        END IF;

                    END IF;

                  IF p_IsCash = 2
                     THEN
                      SELECT ValRate INTO v_Rate FROM
                              BankServicesTb
                      WHERE
                              ID = p_BBRANCHID;
SET v_res = (p_CurrRecievedVal + p_ExVal) * v_Rate;
SET v_MovementUserType = CONCAT(p_BranchRecievedName, ' ', 'لـ', ' ', p_RecievedName);
SET v_BBRTOTALVAL = v_NetTot + p_EXTRVAL;
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_BBRTOTALVAL, 0.000, p_InsertDate, p_Description, p_Code, 27, 2, p_BranchRecievedID, p_BBranchAccID, v_AccFrom, 0, 0, 'حوالة خارجية', p_RecievedCurrencyID, 0, 0, 'حوالة خارجية');
                      IF p_EXTRVAL > 0.000
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_EXTRVAL, p_InsertDate, p_Description, p_Code, 27, 2, p_BranchRecievedID, v_BBRANCHALLOWACCID, p_BBranchAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                    END IF;

                END IF;








            END IF;



          IF p_IsInOrOut = 1
             THEN

              IF (v_DBrType = 1
                OR
                v_DBrType = 2)
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_OverallVal, 0.000, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccTo, v_AccForDel, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccForDel, v_AccTo, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                END IF;

            END IF;


        END IF;




      IF p_ConfirmedType = 1
         THEN


          IF p_IsInOrOut = 0
             THEN
              SELECT IsConfirmed, ConfirmedType INTO v_CheckParm1, v_CheckParm2 FROM
                      ExternalEx
              WHERE
                      Code = p_Code;

              IF v_CheckParm1 = 1
                AND
                v_CheckParm2 = 1
                 THEN
                  SET p_msgIN = 2;
                  SET p_MSG = 'تم إعتماد هذه الحوالة مسبقا';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              UPDATE
                      ExternalEx
              SET
                      IsConfirmed = 1, ConfirmedType = 1, BranchDeleviredID = p_BranchDeliveredID, OurAccID = p_OurAccID, ConfirmedSafeID = p_ConfirmedSafeID, ConfirmDate = NOW(), NewTrancPrice = p_NewTrancPrice, NewFinalTotal = p_NewFinalTotal
              WHERE
                Code = p_Code;


              IF p_AgentCheck = 0
                 THEN
                  SET v_AccToMizan = p_OurAccID;
                END IF;

              IF p_AgentCheck = 2
                 THEN
                  SET v_AccToMizan = v_CurrentDelAccID;
                END IF;

              IF p_BranchRecievedID <> v_MainBr
                AND
                (v_RBrType = 1
                OR
                v_RBrType = 2)
                AND
                v_RBrType <> 3
                AND
                v_RBrType <> 4
                AND
                p_BranchDeliveredID <> v_MainBr
                 THEN
                  SET v_AccToMizan = v_MainAcccurrent;
                END IF;

              IF v_RBrType <> 3
                AND
                v_RBrType <> 4
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NetTot, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccFrom, p_BranchDeliveredID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                END IF;


              SELECT a.CurrencyPower INTO v_CurrPower FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = p_RecievedCurrencyID
                      AND b.PriceType = 2
                      AND b.AccountType = 1
                      AND b.CountryID = p_CountryIDTo
                      AND a.CurrencyIDTo = p_DeliveredCurrencyID;
              SELECT a.TransPrice INTO v_OldPrice FROM
                      ExternalEx AS a
              WHERE
                      a.Code = p_Code;






              IF v_CurrPower = 0
                 THEN
SET v_OldTotalPrice = p_CurrRecievedVal * v_OldPrice;
                END IF;

              IF v_CurrPower = 1
                 THEN
SET v_OldTotalPrice = p_CurrRecievedVal / v_OldPrice;
                END IF;


              SELECT CASE                                            WHEN c.CurrencyPower = 0                                              THEN                                              CASE                                                      WHEN IFNULL(SUM(CRedetDL) - SUM(ACC_DEPET_DL), 0.000) = 0                                                        THEN                                                        0.00                                                      ELSE                                                      IFNULL(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO), 0.000) / IFNULL(SUM(CRedetDL) - SUM(ACC_DEPET_DL), 0.000)                                              END                                            ELSE                                            CASE                                                    WHEN IFNULL(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO), 0.000) = 0                                                      THEN                                                      0.00                                                    ELSE                                                    IFNULL(SUM(CRedetDL) - SUM(ACC_DEPET_DL), 0.000) / IFNULL(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO), 0.000)                                            END                                    END INTO v_CostPrice FROM
                      ExSyAccountsCurrency_CurrencymovementPruse AS
                      a
                  INNER JOIN
                    CurrencyMainTb AS b
                      ON a.AccFrom = b.ID
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS c
                      ON b.ID = c.CurrencyIDTo
              WHERE
                      a.IsActive = 1

                      AND a.AccID = p_OurAccID
                      AND a.AccFrom = p_DeliveredCurrencyID


              GROUP BY
                      b.CuName,
                      c.CurrencyPower;

              IF v_CurrPower = 0
                 THEN
SET v_NewPrice = v_OldTotalPrice / v_CostPrice;
                END IF;

              IF v_CurrPower = 1
                 THEN
SET v_NewPrice = v_OldTotalPrice * v_CostPrice;
                END IF;




              IF p_AgentCheck = 0
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NewPrice, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, p_OurAccID, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                  CALL ExSyAccountsCurrency_AccSafeActivityTb_Inert(p_SafeRecievedID, v_OldTotalPrice, 0, p_ConfirmDate, p_Notes, p_Code, 1, 1, v_MainBr, p_OurAccID, p_BranchDeliveredID, 0, 0, 'بيع عملة مقابل دينار الليبي', p_DeliveredCurrencyID, 0, 1, '');
                  INSERT INTO ExSyAccountsCurrency_CurrencymovementPruse
                        (
                          `ISID`,
                          `AccFrom`,
                          `ACCCRint0`,
                          `CRedetTO`,
                          `CRedetDL`,
                          `ACC_DEPET_DL`,
                          `ACC_DEPET_TO`,
                          `UESER_INSERt`,
                          `Inseart_Date`,
                          `IsActive`,
                          `BID`,
                          `TYPEFROM`,
                          Purchaseprice,
                          AccID,
                          Typeofsalebuyorsell,
                          NetSale,
                          salesPurchaseprice,
                          BankID,
                          CountryID
                        )
                  VALUES
                          (
                            p_Code, p_DeliveredCurrencyID, p_RecievedCurrencyID, 0.000, 0.00, v_NewPrice, v_OldTotalPrice, p_SafeRecievedID, NOW(), 1, v_MainBr, 2, v_CostPrice, p_OurAccID, 1, p_CurrRecievedVal - v_NewPrice, p_TransPrice, 0, p_CountryIDTo
                          );
SET v_BounsSaleVal = p_CurrRecievedVal - v_NewPrice;
                  IF v_BounsSaleVal > 0
                     THEN
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_BounsSaleVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccBounsSale, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                    END IF;


                  IF p_BranchRecievedID = v_MainBr
                     THEN

                      IF p_ExVal > 0
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_ExVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccIncome, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                    END IF;



                  IF p_BranchRecievedID <> v_MainBr
                    AND
                    (v_RBrType = 1
                    OR
                    v_RBrType = 2)
                    AND
                    v_RBrType <> 3
                    AND
                    v_RBrType <> 4
                    AND
                    p_BranchDeliveredID <> v_MainBr
                     THEN

                      IF v_RBShare > 0
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RBShare, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccIncome, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;


                      IF v_RestVal > 0
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccMainFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                      SET v_RestVal = p_CurrRecievedVal + v_RBShare;
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_MainAcccurrent, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, p_OurAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                    END IF;



                  IF p_BranchRecievedID <> v_MainBr
                    AND
                    v_RBrType <> 1
                    AND
                    v_RBrType <> 2
                    AND
                    v_RBrType = 3
                    AND
                    v_RBrType <> 4
                    AND
                    p_AgentCheck = 0
                     THEN


                      IF v_RestVal > 0
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccMainFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                      SET v_RestVal = (p_ExVal - v_RBShare);
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_CurrRecievedVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, p_OurAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, p_OurAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                    END IF;


                END IF;





              IF p_AgentCheck = 2
                 THEN

                  IF v_CurrPower IS NULL
                     THEN
                      SET p_msgIN = 0;
                      SET p_MSG = 'عذرا  لايمكن اعتماد هذه الحوالة ';
                      ROLLBACK;
                      LEAVE proc;

                    END IF;



                  SELECT a.TransPrice1 INTO v_OldPrice1 FROM
                          ExternalEx AS a
                  WHERE
                          a.Code = p_Code;

                  IF v_CurrPower = 0
                     THEN
SET v_OldTotalPrice1 = p_CurrRecievedVal * v_OldPrice1;
                    END IF;

                  IF v_CurrPower = 1
                     THEN
SET v_OldTotalPrice1 = p_CurrRecievedVal / v_OldPrice1;
                    END IF;



                  IF v_CurrPower = 0



                     THEN
SET v_NewPrice  = FLOOR(v_OldTotalPrice / p_TransPrice);
SET v_NewPrice1 = FLOOR(v_OldTotalPrice1 / p_TransPrice);
                    END IF;

                  IF v_CurrPower = 1
                     THEN
SET v_NewPrice  = FLOOR(v_OldTotalPrice * p_TransPrice);
SET v_NewPrice1 = FLOOR(v_OldTotalPrice1 * p_TransPrice);
                    END IF;
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                  IF v_NewPrice IS NULL
                     THEN
                      SET p_msgIN = 0;
                      SET p_MSG = 'عذر ا لايمكن ان تكون قيمة الوكيل فارغة ';
                      ROLLBACK;
                      LEAVE proc;
                    END IF;

                  UPDATE
                          ExternalEx
                  SET
                          NewCurrRecievedVal = v_NewPrice1
                  WHERE
                    Code = p_Code;


                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NewPrice1, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentDelAccID, v_CurrentAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                  IF v_NewPrice < p_CurrRecievedVal
                     THEN
                      SET v_RevenuesOrLossesVal = p_CurrRecievedVal - v_NewPrice;
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RevenuesOrLossesVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccRevenues, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                    END IF;


                  IF v_NewPrice > p_CurrRecievedVal
                     THEN
                      SET v_RevenuesOrLossesVal = v_NewPrice - p_CurrRecievedVal;
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RevenuesOrLossesVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccLosses, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                    END IF;

                  SET p_NewPrice_SenDForWhatsapp = v_NewPrice1;
                  IF p_BranchRecievedID = v_MainBr
                     THEN

                      IF p_ExVal > 0
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_ExVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccIncome, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                    END IF;



                  IF p_BranchRecievedID <> v_MainBr
                    AND
                    (v_RBrType = 1
                    OR
                    v_RBrType = 2)
                    AND
                    (v_RBrType <> 3
                    OR
                    v_RBrType <> 4)
                     THEN

                      IF v_RBShare > 0
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RBShare, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccIncome, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;


                      IF v_RestVal > 0
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccMainFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                      SET v_RestVal = p_CurrRecievedVal + (p_ExVal - v_RBShare);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_MainAcccurrent, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                      IF v_OldPrice1 <> v_OldPrice
                         THEN
                          SET v_RestVal = v_NewPrice - v_NewPrice1;
                          IF v_NewPrice1 < p_CurrRecievedVal
                             THEN


                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_MainAcccurrent, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccRevenues1, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;


                          IF v_NewPrice1 > p_CurrRecievedVal
                             THEN


                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_MainAcccurrent, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccLosses1, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccRevenues, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;
                        END IF;
                    END IF;



                  IF p_BranchRecievedID <> v_MainBr
                    AND
                    v_RBrType <> 1
                    AND
                    v_RBrType <> 2
                    AND
                    v_RBrType = 3
                    AND
                    v_RBrType <> 4
                    AND
                    v_DBrType <> 1
                    AND
                    v_DBrType <> 2
                    AND
                    p_AgentCheck = 2
                     THEN


                      IF v_RestVal > 0
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccMainFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;

                      SET v_RestVal = p_CurrRecievedVal + (p_ExVal - v_RBShare);
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                    END IF;


                END IF;


            END IF;



          IF p_IsInOrOut = 1
             THEN

              UPDATE
                      ExternalEx
              SET
                      IsConfirmed = 1, ConfirmedType = 1, ConfirmedSafeID = p_ConfirmedSafeID, ConfirmDate = p_ConfirmDate
              WHERE
                Code = p_Code;

              IF v_DBrType = 3
                 THEN

                  IF (v_RBrType = 0
                    OR
                    v_RBrType IS NULL)
                     THEN
                      SET p_AgentCheck = 0;
                    END IF;

                  IF v_RBrType = 4
                    OR
                    v_RBrType = 3
                     THEN
                      SET p_AgentCheck = 1;
                    END IF;

                  IF p_AgentCheck = 0
                     THEN
                      SELECT a.CurrencyPower, a.BuyPrice INTO v_CurrPower, v_OldPrice FROM
                              NewCurrencyPriceOwnDetailsTb AS a
                          INNER JOIN
                            NewCurrencyPricesOwnTb AS b
                              ON a.CPID = b.ID
                      WHERE
                              a.CurrencyIDFrom = p_DeliveredCurrencyID
                              AND b.PriceType = 1
                              AND b.AccountType = 0
                              AND b.CountryID = p_CountryIDFrom
                              AND a.CurrencyIDTo = p_RecievedCurrencyID;


                      IF v_CurrPower = 0
                         THEN
SET v_OldTotalPrice = p_CurrRecievedVal / v_OldPrice;
                        END IF;

                      IF v_CurrPower = 1
                         THEN
SET v_OldTotalPrice = p_CurrRecievedVal * v_OldPrice;
                        END IF;

                      IF v_OldTotalPrice > p_NewFinalTotal
                         THEN
                          SET v_RevenuesOrLossesVal = v_OldTotalPrice - p_NewFinalTotal - p_HandelExAVal;
                          SET v_RestVal = p_OverallVal + p_HandelExAVal;
                        END IF;

                      IF v_OldTotalPrice < p_NewFinalTotal
                         THEN
                          SET v_RevenuesOrLossesVal = p_NewFinalTotal - v_OldTotalPrice + p_HandelExAVal;
                          SET v_RestVal = p_OverallVal + p_HandelExAVal;
                        END IF;

                      SET v_AccToMizan = v_CurrentDelAccID;
                      IF p_BranchDeliveredID = v_MainBr
                         THEN
                          SET v_AccToMizan = v_AccTo;
                        END IF;


                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_OldTotalPrice, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, p_BranchRecievedID, v_AccToMizan, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentDelAccID, p_BranchRecievedID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_HandelExAVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentDelAccID, p_BranchRecievedID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                      CALL ExSyAccountsCurrency_AccSafeActivityTb_Inert(p_SafeRecievedID, 0.000, p_CurrRecievedVal, p_ConfirmDate, p_Notes, p_Code, 1, 1, v_MainBr, p_BranchRecievedID, p_BranchDeliveredID, 0, 0, 'بيع عملة مقابل دينار الليبي', p_RecievedCurrencyID, 0, 1, '');
                      INSERT INTO ExSyAccountsCurrency_CurrencymovementPruse
                            (
                              `ISID`,
                              `AccFrom`,
                              `ACCCRint0`,
                              `CRedetTO`,
                              `CRedetDL`,
                              `ACC_DEPET_DL`,
                              `ACC_DEPET_TO`,
                              `UESER_INSERt`,
                              `Inseart_Date`,
                              `IsActive`,
                              `BID`,
                              `TYPEFROM`,
                              Purchaseprice,
                              AccID,
                              Typeofsalebuyorsell,
                              salesPurchaseprice,
                              BankID,
                              CountryID
                            )
                      VALUES
                              (
                                p_Code, p_RecievedCurrencyID, p_DeliveredCurrencyID, p_CurrRecievedVal, v_OldTotalPrice, 0.000, 0.000, p_SafeRecievedID, NOW(), 1, v_MainBr, 2, v_OldPrice, p_BranchRecievedID, 1, v_OldPrice, 0, p_CountryIDTo
                              );




                      IF v_OldTotalPrice > p_NewFinalTotal
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RevenuesOrLossesVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccRevenues, p_BranchRecievedID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;


                      IF v_OldTotalPrice < p_NewFinalTotal
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RevenuesOrLossesVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccLosses, p_BranchRecievedID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;


                    END IF;

                  IF p_AgentCheck = 1
                     THEN

                      SELECT a.CurrencyPower, a.BuyPrice INTO v_CurrPower, v_OldPrice FROM
                              NewCurrencyPriceOwnDetailsTb AS a
                          INNER JOIN
                            NewCurrencyPricesOwnTb AS b
                              ON a.CPID = b.ID
                      WHERE
                              a.CurrencyIDFrom = p_DeliveredCurrencyID
                              AND b.PriceType = 2
                              AND b.AccountType = 1
                              AND b.CountryID = p_CountryIDFrom
                              AND a.CurrencyIDTo = p_RecievedCurrencyID
                              AND b.BranchID = p_BranchRecievedID;

                      IF v_CurrPower = 0
                         THEN
SET v_OldTotalPrice = FLOOR(p_CurrRecievedVal / v_OldPrice);
                        END IF;

                      IF v_CurrPower = 1
                         THEN
SET v_OldTotalPrice = FLOOR(p_CurrRecievedVal * v_OldPrice);
                        END IF;
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_OldTotalPrice, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                      SET v_RestVal = p_OverallVal + p_HandelExAVal;
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentDelAccID, v_CurrentAccID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_HandelExAVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentDelAccID, v_CurrentAccID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                      IF v_OldTotalPrice > v_RestVal
                         THEN
                          SET v_RevenuesOrLossesVal = v_OldTotalPrice - v_RestVal;
                        END IF;

                      IF v_OldTotalPrice < v_RestVal
                         THEN
                          SET v_RevenuesOrLossesVal = v_RestVal - v_OldTotalPrice;
                        END IF;



                      IF v_OldTotalPrice > v_RestVal
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RevenuesOrLossesVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccRevenues, v_CurrentAccID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;


                      IF v_OldTotalPrice < v_RestVal
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RevenuesOrLossesVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccLosses, v_CurrentAccID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;


                    END IF;

                END IF;

              IF v_RBrType = 4
                OR
                v_RBrType = 3
                AND
                v_DBrType <> 3
                 THEN

                  IF (v_RBrType = 0
                    OR
                    v_RBrType IS NULL)
                     THEN
                      SET p_AgentCheck = 0;
                    END IF;

                  IF v_RBrType = 4
                    OR
                    v_RBrType = 3
                     THEN
                      SET p_AgentCheck = 1;
                    END IF;

                  IF p_AgentCheck = 1
                     THEN

                      SET v_AccToMizan = v_CurrentAccID;
                      IF p_BranchDeliveredID <> v_MainBr
                        AND
                        v_DBrType <> 3
                         THEN
                          SET v_AccToMizan = v_MainAcccurrent;
                        END IF;


                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchDeliveredID, v_AccTo, v_AccToMizan, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                      SELECT a.CurrencyPower, a.BuyPrice INTO v_CurrPower, v_OldPrice FROM
                              NewCurrencyPriceOwnDetailsTb AS a
                          INNER JOIN
                            NewCurrencyPricesOwnTb AS b
                              ON a.CPID = b.ID
                      WHERE
                              a.CurrencyIDFrom = p_DeliveredCurrencyID
                              AND b.PriceType = 2
                              AND b.AccountType = 1
                              AND b.CountryID = p_CountryIDFrom
                              AND a.CurrencyIDTo = p_RecievedCurrencyID
                              AND b.BranchID = p_BranchRecievedID;

                      IF v_CurrPower = 0
                         THEN
SET v_OldTotalPrice = FLOOR(p_CurrRecievedVal / v_OldPrice);
                        END IF;

                      IF v_CurrPower = 1
                         THEN
SET v_OldTotalPrice = FLOOR(p_CurrRecievedVal * v_OldPrice);
                        END IF;

                      IF v_OldTotalPrice > p_NewFinalTotal
                         THEN
                          SET v_RevenuesOrLossesVal = v_OldTotalPrice - p_NewFinalTotal;
                        END IF;

                      IF v_OldTotalPrice < p_NewFinalTotal
                         THEN
                          SET v_RevenuesOrLossesVal = p_NewFinalTotal - v_OldTotalPrice;
                        END IF;
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      SET v_AccToMizan = v_AccTo;
                      IF p_BranchDeliveredID <> v_MainBr
                        AND
                        v_DBrType <> 3
                         THEN
                          SET v_AccToMizan = v_CurrentDelAccID;
                        END IF;


                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_OldTotalPrice, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_AccToMizan, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                      IF p_BranchDeliveredID <> v_MainBr
                        AND
                        v_DBrType <> 3
                         THEN



                          IF (p_OverallVal * 0.01) > v_RevenuesOrLossesVal
                             THEN
                              SET v_NewPrice = FLOOR(v_RevenuesOrLossesVal / 100 * v_DRateWithMain);
                              SET v_RevenuesOrLossesVal = v_RevenuesOrLossesVal - v_NewPrice;
                            END IF;

                          IF (p_OverallVal * 0.01) < v_RevenuesOrLossesVal
                             THEN
                              SET v_NewPrice = FLOOR((p_OverallVal * 0.01) / 100 * v_DRateWithMain);
                              SET v_RevenuesOrLossesVal = v_RevenuesOrLossesVal - v_NewPrice;
                            END IF;

                          IF v_NewPrice > 0
                             THEN

                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NewPrice, p_InsertDate, p_Description, p_Code, 2, 2, p_BranchDeliveredID, v_AccOutcome, v_AccTo, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                            END IF;

                          SET v_RestVal = p_OverallVal + v_NewPrice;
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchDeliveredID, v_MainAcccurrent, v_AccTo, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                          SET v_RestVal = p_OverallVal + v_NewPrice;
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentDelAccID, v_CurrentAccID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                        END IF;




                      IF v_OldTotalPrice < p_NewFinalTotal
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RevenuesOrLossesVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccLosses, v_CurrentAccID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;


                      IF v_OldTotalPrice > p_NewFinalTotal
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RevenuesOrLossesVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccRevenues, v_CurrentAccID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;


                    END IF;

                END IF;

              SELECT a.IsAccTo INTO p_IsAccTo FROM
                      ExternalEx AS a
              WHERE
                      a.Code = p_Code;

              IF p_IsAccTo = 1
                 THEN
                  SET v_goto_IsHasAccTO = 1;   -- jump replaced by flag
                END IF;

            END IF;


        END IF;





      IF (p_ConfirmedType = 2 OR v_goto_IsHasAccTO = 1)
         THEN

          -- This inner gate is deliberately NOT widened with the flag. The jump can only fire from
          -- inside "IF @IsInOrOut = 1" (T-SQL line 2012), so whenever v_goto_IsHasAccTO = 1 the
          -- condition below is already true; widening it would change nothing except hide that fact.
          IF p_IsInOrOut = 1
             THEN
              -- (label removed; reached via v_goto_IsHasAccTO)
              UPDATE
                      ExternalEx
              SET
                      IsConfirmed = 2, ConfirmedType = 2, ConfirmedSafeID = p_ConfirmedSafeID, RecievedDate = p_RecievedDate, IsDelivered = 1
              WHERE
                Code = p_Code;

              IF (v_RBrType = 0
                OR
                v_RBrType IS NULL)
                 THEN
                  SET p_AgentCheck = 0;
                END IF;

              IF v_RBrType = 4
                OR
                v_RBrType = 3
                 THEN
                  SET p_AgentCheck = 1;
                END IF;

              SELECT a.IsAccTo INTO p_IsAccTo FROM
                      ExternalEx AS a
              WHERE
                      a.Code = p_Code;

              IF p_IsAccTo = 1
                 THEN
                  SELECT a.AccTo INTO v_AccUserID FROM
                          ExternalEx AS a
                  WHERE
                          a.Code = p_Code;
                END IF;


              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_OverallVal, 0.000, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccForDel, v_AccUserID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
              IF p_AgentCheck = 0
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccTo, p_BranchRecievedID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                END IF;
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_InsertDate, p_Notes, p_Code, 2, 27, p_BranchDeliveredID, v_AccUserID, v_AccForDel, 0, 0, p_BranchRecievedName, p_DeliveredCurrencyID, 0, 0, v_MovementType);
              IF p_AgentCheck = 0
                 THEN
                  SELECT a.CurrencyPower, a.BuyPrice INTO v_CurrPower, v_OldPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = p_DeliveredCurrencyID
                          AND b.PriceType = 1
                          AND b.AccountType = 0
                          AND b.CountryID = p_CountryIDFrom
                          AND a.CurrencyIDTo = p_RecievedCurrencyID;


                  IF v_CurrPower = 0
                     THEN
SET v_OldTotalPrice = p_CurrRecievedVal / v_OldPrice;
                    END IF;

                  IF v_CurrPower = 1
                     THEN
SET v_OldTotalPrice = p_CurrRecievedVal * v_OldPrice;
                    END IF;

                  IF v_OldTotalPrice > p_NewFinalTotal
                     THEN
                      SET v_RevenuesOrLossesVal = v_OldTotalPrice - p_NewFinalTotal;
                    END IF;

                  IF v_OldTotalPrice < p_NewFinalTotal
                     THEN
                      SET v_RevenuesOrLossesVal = p_NewFinalTotal - v_OldTotalPrice;
                    END IF;

                  IF p_BranchDeliveredID <> v_MainBr
                    AND
                    v_DBrType <> 3
                     THEN



                      IF (p_OverallVal * 0.01) > v_RevenuesOrLossesVal
                         THEN
                          SET v_NewPrice = FLOOR(v_RevenuesOrLossesVal / 100 * v_DRateWithMain);
                          SET v_RevenuesOrLossesVal = v_RevenuesOrLossesVal - v_NewPrice;
                        END IF;

                      IF (p_OverallVal * 0.01) < v_RevenuesOrLossesVal
                         THEN
                          SET v_NewPrice = FLOOR((p_OverallVal * 0.01) / 100 * v_DRateWithMain);
                          SET v_RevenuesOrLossesVal = v_RevenuesOrLossesVal - v_NewPrice;
                        END IF;

                      IF v_NewPrice > 0
                         THEN

                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NewPrice, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccOutcome, v_AccTo, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                        END IF;

                      SET v_RestVal = p_OverallVal + v_NewPrice;
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_MainAcccurrent, v_AccTo, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                      SET v_RestVal = p_OverallVal + v_NewPrice;
SET v_MovementType = CONCAT('حوالة خارجية ', SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_CurrentDelAccID, v_AccUserID, 0, 0, v_MovementType, p_DeliveredCurrencyID, 0, 0, '');
                    END IF;



                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_OldTotalPrice, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, p_BranchRecievedID, v_CurrentDelAccID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                  CALL ExSyAccountsCurrency_AccSafeActivityTb_Inert(p_SafeRecievedID, 0.000, p_CurrRecievedVal, p_ConfirmDate, p_Notes, p_Code, 1, 1, v_MainBr, p_BranchRecievedID, p_BranchDeliveredID, 0, 0, 'بيع عملة مقابل دينار الليبي', p_RecievedCurrencyID, 0, 1, '');
                  INSERT INTO ExSyAccountsCurrency_CurrencymovementPruse
                        (
                          `ISID`,
                          `AccFrom`,
                          `ACCCRint0`,
                          `CRedetTO`,
                          `CRedetDL`,
                          `ACC_DEPET_DL`,
                          `ACC_DEPET_TO`,
                          `UESER_INSERt`,
                          `Inseart_Date`,
                          `IsActive`,
                          `BID`,
                          `TYPEFROM`,
                          Purchaseprice,
                          AccID,
                          Typeofsalebuyorsell,
                          salesPurchaseprice,
                          BankID,
                          CountryID
                        )
                  VALUES
                          (
                            p_Code, p_RecievedCurrencyID, p_DeliveredCurrencyID, p_CurrRecievedVal, v_OldTotalPrice, 0.000, 0.000, p_SafeRecievedID, NOW(), 1, v_MainBr, 2, v_OldPrice, p_BranchRecievedID, 1, v_OldPrice, 0, p_CountryIDFrom
                          );





                  IF v_OldTotalPrice > p_NewFinalTotal
                     THEN

                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RevenuesOrLossesVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccRevenues, p_BranchRecievedID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                    END IF;


                  IF v_OldTotalPrice < p_NewFinalTotal
                     THEN
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RevenuesOrLossesVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccLosses, p_BranchRecievedID, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                    END IF;


                END IF;

            END IF;

        END IF;



      IF p_ConfirmedType = 3
         THEN

          SELECT IsConfirmed, OurAccID INTO v_IsConf, p_OurAccID FROM
                  ExternalEx
          WHERE
                  Code = p_Code;


          IF p_IsInOrOut = 0
             THEN

              IF (v_RBrType = 1
                OR
                v_RBrType = 2)
                 THEN

                  IF p_IsAccFrom = 0
                     THEN
                      SET v_AccToMizan = v_AccUserID;
                    END IF;

                  IF p_IsAccFrom = 1
                     THEN
                      SET v_AccToMizan = p_AccFID;
                    END IF;


                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NetTot, 0.000, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccCancleFrom, v_AccToMizan, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                END IF;

              IF (v_RBrType = 1
                OR
                v_RBrType = 2)
                AND
                v_RBrType <> 3
                AND
                v_RBrType <> 4

                 THEN

                  IF p_IsCash = 0
                     THEN
SET v_MovementType = CONCAT('حوالة خارجية ملغية من/', SPACE(1), p_SenderName, SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NetTot, p_InsertDate, p_Description, p_Code, 2, 27, p_BranchRecievedID, v_AccUserID, v_AccCancleFrom, 0, 0, p_BranchRecievedName, p_RecievedCurrencyID, 0, 0, v_MovementType);
                    END IF;

                  IF (p_IsCash = 1
                    OR
                    p_IsCash = 0)
                     THEN
                      SELECT IsAccFrom INTO p_IsAccFrom FROM
                              ExternalEx
                      WHERE
                              Code = p_Code;

                      IF p_IsAccFrom = 1
                        AND
                        p_IsAccTo = 0
                         THEN

                          IF p_EMPCUSTSELECT = 5
                             THEN
SET v_OPETYPE = 38;
SET v_TPID = 5;
                              SELECT a.ID, a.AccID INTO v_CUSTEMPID, p_AccFID FROM
                                      EmployeeTb AS a
                              WHERE
                                      a.EMPNAME = p_SenderName;
                            END IF;

                          IF p_EMPCUSTSELECT = 6
                             THEN
SET v_OPETYPE = 39;
SET v_TPID = 6;
                              SELECT a.ID, a.AccID INTO v_CUSTEMPID, p_AccFID FROM
                                      CustomersTb AS a
                              WHERE
                                      a.CustName = p_SenderName;
                            END IF;

                          IF p_EMPCUSTSELECT = 25
                             THEN
SET v_OPETYPE = 54;
SET v_TPID = 25;
                              SELECT a.ID, a.AccID INTO v_CUSTEMPID, p_AccFID FROM
                                      CreditAccountsTb AS a
                              WHERE
                                      a.AccName = p_SenderName;
                            END IF;
SET v_CECODE = CONCAT(CAST(v_TPID AS CHAR), '-', CAST(EMPORCUSTWITHDRAWALTB_GetMaxCODEID(v_TPID) AS CHAR));
SET v_ECCODEID = EMPORCUSTWITHDRAWALTB_GetMaxCODEID(v_TPID);
SET v_WDNOTES = CONCAT('سحب من حساب', CASE                                                                      WHEN p_EMPCUSTSELECT = 5                                                                        THEN                                                                        'الموظف'                                                                      WHEN p_EMPCUSTSELECT = 6                                                                        THEN                                                                        'العميل'                                                                      ELSE                                                                      'المدين'                                                              END, p_SenderName, ' ', 'مقابل حوالة خارجية ملغاة');
SET v_MovementUserType = CONCAT(p_BranchRecievedName, ' ', 'للمستلم', ' ', p_RecievedName);
                          INSERT INTO EMPORCUSTWITHDRAWALTB
                                (
                                  Code,
                                  InsertDate,
                                  EMPID,
                                  WDVAL,
                                  DPSVAL,
                                  SafeID,
                                  IsActive,
                                  TypeID,
                                  CODEID,
                                  BranchID,
                                  Notes,
                                  InterExCode
                                )
                          VALUES
                                  (
                                    v_CECODE, p_InsertDate, v_CUSTEMPID, 0.000, p_CurrRecievedVal, p_SafeRecievedID, 1, v_TPID, v_ECCODEID, p_BranchRecievedID, v_WDNOTES, p_Code
                                  );
SET v_GetMovementUserType = CONCAT('إيداع في حساب', ' ', CASE                                                                                          WHEN p_EMPCUSTSELECT = 5                                                                                            THEN                                                                                            'الموظف'                                                                                          WHEN p_EMPCUSTSELECT = 6                                                                                            THEN                                                                                            'العميل'                                                                                          ELSE                                                                                          'المدين'                                                                                  END, ' ', p_SenderName, ' ', 'مقابل حوالة خارجية ملغاة');
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NetTot, p_InsertDate, p_Description, p_Code, 1, 18, p_BranchRecievedID, p_AccFID, v_AccCancleFrom, 0, 0, 'سحب بتحويل خارجي من حساب', p_RecievedCurrencyID, 0, 0, '');
                          IF p_IsCash = 0
                             THEN

                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NetTot, 0.000, p_InsertDate, p_Description, p_Code, 2, 27, p_BranchRecievedID, v_AccUserID, v_AccCancleFrom, 0, 0, p_BranchRecievedName, p_RecievedCurrencyID, 0, 0, v_GetMovementUserType);
                            END IF;

                          UPDATE
                                  EMPORCUSTWITHDRAWALTB
                          SET
                                  IsActive = 0
                          WHERE
                            Code = p_Code;
                        END IF;

                    END IF;

                END IF;

              IF v_IsConf = 0
                 THEN

                  IF v_RBrType <> 3
                    AND
                    v_RBrType <> 4
                     THEN

                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NetTot, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccFrom, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                    END IF;

                END IF;

              IF v_RBrType <> 3
                AND
                v_RBrType <> 4
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_NetTot, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccCancleFrom, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                END IF;

              IF v_IsConf = 1
                 THEN


                  IF (v_DBrType = 0
                    OR
                    v_DBrType IS NULL)
                     THEN
                      SET p_AgentCheck = 0;
                    END IF;

                  IF v_DBrType = 4
                    OR
                    v_DBrType = 3
                     THEN
                      SET p_AgentCheck = 2;
                    END IF;

                  SELECT a.CurrencyPower INTO v_CurrPower FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = p_RecievedCurrencyID
                          AND b.PriceType = 2
                          AND b.AccountType = 1
                          AND b.CountryID = p_CountryIDTo
                          AND a.CurrencyIDTo = p_DeliveredCurrencyID;
                  SELECT a.TransPrice INTO v_OldPrice FROM
                          ExternalEx AS a
                  WHERE
                          a.Code = p_Code;

                  IF v_CurrPower = 0
                     THEN
SET v_OldTotalPrice = p_CurrRecievedVal * v_OldPrice;
                    END IF;

                  IF v_CurrPower = 1
                     THEN
SET v_OldTotalPrice = p_CurrRecievedVal / v_OldPrice;
                    END IF;


                  SELECT Purchaseprice INTO v_CostPrice FROM
                          ExSyAccountsCurrency_CurrencymovementPruse
                  WHERE
                          ISID = p_Code;

                  IF v_CurrPower = 0
                     THEN
SET v_NewPrice = v_OldTotalPrice / v_CostPrice;
                    END IF;

                  IF v_CurrPower = 1
                     THEN
SET v_NewPrice = v_OldTotalPrice * v_CostPrice;
                    END IF;


                  IF p_AgentCheck = 0
                     THEN

                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NewPrice, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, p_OurAccID, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                      CALL ExSyAccountsCurrency_AccSafeActivityTb_Inert(p_SafeRecievedID, 0, v_OldTotalPrice, p_ConfirmDate, p_Notes, p_Code, 1, 1, v_MainBr, p_OurAccID, p_BranchDeliveredID, 0, 0, 'استرجاع بيع عملة مقابل دينار الليبي', p_DeliveredCurrencyID, 0, 0, '');
                      INSERT INTO ExSyAccountsCurrency_CurrencymovementPruse
                            (
                              `ISID`,
                              `AccFrom`,
                              `ACCCRint0`,
                              `CRedetTO`,
                              `CRedetDL`,
                              `ACC_DEPET_DL`,
                              `ACC_DEPET_TO`,
                              `UESER_INSERt`,
                              `Inseart_Date`,
                              `IsActive`,
                              `BID`,
                              `TYPEFROM`,
                              Purchaseprice,
                              AccID,
                              Typeofsalebuyorsell,
                              NetSale,
                              salesPurchaseprice,
                              BankID,
                              CountryID
                            )
                      VALUES
                              (
                                p_Code, p_DeliveredCurrencyID, p_RecievedCurrencyID, v_OldTotalPrice, v_NewPrice, 0.000, 0.00, p_SafeRecievedID, NOW(), 1, v_MainBr, 2, v_CostPrice, p_OurAccID, 2, v_NewPrice - p_CurrRecievedVal, p_TransPrice, 0, p_CountryIDTo
                              );

                      SET v_AccToMizan = v_CurrentAccID;
                      IF p_BranchRecievedID = v_MainBr
                         THEN
                          SET v_AccToMizan = v_AccCancleFrom;
                        END IF;
SET v_BounsSaleVal = p_CurrRecievedVal - v_NewPrice;
                      IF v_BounsSaleVal > 0
                         THEN
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_BounsSaleVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccloseSale, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;


                      IF p_BranchRecievedID = v_MainBr
                         THEN

                          IF p_ExVal > 0
                             THEN

                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_ExVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccOutRcome, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;

                        END IF;



                      IF p_BranchRecievedID <> v_MainBr
                        AND
                        (v_RBrType = 1
                        OR
                        v_RBrType = 2)
                        AND
                        v_RBrType <> 3
                        AND
                        v_RBrType <> 4
                        AND
                        p_BranchDeliveredID <> v_MainBr
                         THEN

                          IF v_RBShare > 0
                             THEN

                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RBShare, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccOutRcome, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;


                          IF v_RestVal > 0
                             THEN
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccMainLoseFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;

                          SET v_RestVal = p_CurrRecievedVal + v_RBShare;
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_MainAcccurrent, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_CurrentAccID, p_OurAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;



                      IF p_BranchRecievedID <> v_MainBr
                        AND
                        v_RBrType <> 1
                        AND
                        v_RBrType <> 2
                        AND
                        v_RBrType = 3
                        AND
                        v_RBrType <> 4
                        AND
                        p_AgentCheck = 0
                         THEN


                          IF v_RestVal > 0
                             THEN
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccMainLoseFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;

                          SET v_RestVal = p_CurrRecievedVal + (p_ExVal - v_RBShare);
SET v_MovementType = CONCAT('حوالة خارجية ملغية من/', SPACE(1), p_SenderName, SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_CurrentAccID, p_OurAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                        END IF;


                    END IF;


                  IF p_AgentCheck = 2
                     THEN

                      SELECT a.CurrencyPower INTO v_CurrPower FROM
                              NewCurrencyPriceOwnDetailsTb AS a
                          INNER JOIN
                            NewCurrencyPricesOwnTb AS b
                              ON a.CPID = b.ID
                      WHERE
                              a.CurrencyIDFrom = p_RecievedCurrencyID
                              AND b.PriceType = 2
                              AND b.AccountType = 1
                              AND b.CountryID = p_CountryIDTo
                              AND a.CurrencyIDTo = p_DeliveredCurrencyID;
                      SELECT a.NewTrancPrice INTO v_OldPrice FROM
                              ExternalEx AS a
                      WHERE
                              a.Code = p_Code;



                      SELECT a.TransPrice1 INTO v_OldPrice1 FROM
                              ExternalEx AS a
                      WHERE
                              a.Code = p_Code;

                      SELECT a.TransPrice INTO v_OldPrice2 FROM
                              ExternalEx AS a
                      WHERE
                              a.Code = p_Code;

                      IF v_CurrPower = 0
                         THEN
SET v_OldTotalPrice1 = p_CurrRecievedVal * v_OldPrice1;
                        END IF;

                      IF v_CurrPower = 1
                         THEN
SET v_OldTotalPrice1 = p_CurrRecievedVal / v_OldPrice1;
                        END IF;



                      IF v_CurrPower = 0



                         THEN
SET v_NewPrice  = FLOOR(v_OldTotalPrice / v_OldPrice);
SET v_NewPrice1 = FLOOR(v_OldTotalPrice1 / v_OldPrice);
                        END IF;

                      IF v_CurrPower = 1
                         THEN
SET v_NewPrice  = FLOOR(v_OldTotalPrice * v_OldPrice);
SET v_NewPrice1 = FLOOR(v_OldTotalPrice1 * v_OldPrice);
                        END IF;
SET v_MovementType = CONCAT('حوالة خارجية ملغية من/', SPACE(1), p_SenderName, SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                      SET v_AccToMizan = v_CurrentAccID;
                      IF p_BranchRecievedID = v_MainBr
                         THEN
                          SET v_AccToMizan = v_AccCancleFrom;
                        END IF;


                      CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_NewPrice1, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_CurrentDelAccID, v_AccToMizan, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                      IF v_NewPrice < p_CurrRecievedVal
                         THEN
                          SET v_RevenuesOrLossesVal = p_CurrRecievedVal - v_NewPrice;
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RevenuesOrLossesVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccLosses, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;


                      IF v_NewPrice > p_CurrRecievedVal
                         THEN
                          SET v_RevenuesOrLossesVal = v_NewPrice - p_CurrRecievedVal;
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RevenuesOrLossesVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccRevenues, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                        END IF;




                      IF p_BranchRecievedID = v_MainBr
                         THEN

                          IF p_ExVal > 0
                             THEN

                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_ExVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccOutRcome, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;

                        END IF;



                      IF p_BranchRecievedID <> v_MainBr
                        AND
                        (v_RBrType = 1
                        OR
                        v_RBrType = 2)
                        AND
                        (v_RBrType <> 3
                        OR
                        v_RBrType <> 4)
                         THEN

                          IF v_RBShare > 0
                             THEN

                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RBShare, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_AccOutRcome, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;


                          IF v_RestVal > 0
                             THEN
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_AccMainLoseFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;

                          SET v_RestVal = p_CurrRecievedVal + (p_ExVal - v_RBShare);
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 1, p_BranchRecievedID, v_MainAcccurrent, v_AccCancleFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 1, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                          IF v_OldPrice1 <> v_OldPrice2
                             THEN
                              SET v_RestVal = v_NewPrice - v_NewPrice1;
                              IF v_NewPrice1 < p_CurrRecievedVal
                                 THEN


                                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_MainAcccurrent, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, p_BranchRecievedID, v_AccLosses1, v_AccFrom, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                                END IF;


                            END IF;
                        END IF;



                      IF p_BranchRecievedID <> v_MainBr
                        AND
                        v_RBrType <> 1
                        AND
                        v_RBrType <> 2
                        AND
                        v_RBrType = 3
                        AND
                        v_RBrType <> 4
                        AND
                        p_AgentCheck = 2
                         THEN


                          IF v_RestVal > 0
                             THEN
                              CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, v_RestVal, 0.000, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_AccMainLoseFromBranches, v_CurrentAccID, 0, 0, '', p_RecievedCurrencyID, 0, 0, '');
                            END IF;

                          SET v_RestVal = (p_ExVal - v_RBShare);
SET v_MovementType = CONCAT('حوالة خارجية ملغية من/', SPACE(1), p_SenderName, SPACE(1), 'لـ/', SPACE(1), p_RecievedName);
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_CurrRecievedVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                          CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, v_RestVal, p_ConfirmDate, p_Description, p_Code, 2, 2, v_MainBr, v_CurrentAccID, v_CurrentDelAccID, 0, 0, v_MovementType, p_RecievedCurrencyID, 0, 0, '');
                        END IF;


                    END IF;

                END IF;


            END IF;




          IF p_IsInOrOut = 1
             THEN

              IF (v_DBrType = 1
                OR
                v_DBrType = 2)
                 THEN

                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, 0.000, p_OverallVal, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccTo, v_AccForDel, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                  CALL ExSyAccounts_AccSafeActivityTb_Insert(p_SafeRecievedID, p_OverallVal, 0.000, p_InsertDate, p_Description, p_Code, 2, 1, p_BranchDeliveredID, v_AccForDel, v_AccTo, 0, 0, '', p_DeliveredCurrencyID, 0, 0, '');
                END IF;

            END IF;


          UPDATE
                  ExternalEx
          SET
                  IsCanceled = 1, IsConfirmed = 3, ConfirmedType = 6
          WHERE
            Code = p_Code;
        END IF;


      COMMIT;
      SET p_msgIN = 1;
  END
$$
DELIMITER ;
