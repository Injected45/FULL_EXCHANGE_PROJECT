-- 14 PROCEDURE(s) — SQL Server -> MariaDB/MySQL. SYNTACTICALLY CONVERTED + CREATED, but
-- NOT behaviorally diff-verified (these write data; verify with curated inputs before trusting).
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';   -- allows 'func (x)' spacing in legacy T-SQL

DROP PROCEDURE IF EXISTS `Banck_central_shee_Type_insert`;
DELIMITER //
CREATE PROCEDURE `Banck_central_shee_Type_insert`()
BEGIN
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK;  END;
START TRANSACTION;


INSERT INTO `Banck_central_shee`
           (`REFERENCE`
           ,`TYPE`
           ,`PHONE`
           ,`IBAN`
           ,`BANK_NAME`
           ,`CASH_PRICE`
           ,`BANK_TRANSFER_PRICE`
           ,`AMOUNT_REQUESTED`
           ,`COST`
           ,`UiseridImpotr`
           ,`insertDate`)


           SELECT a.REFERENCE , a.TYPE ,a.PHONE ,a.IBAN ,a.BANK_NAME ,a.CASH_PRICE ,a.BANK_TRANSFER_PRICE ,a.AMOUNT_REQUESTED ,a.COST ,a.UiseridImpotr ,a.insertDate
           FROM tvp_TYPE AS A;

COMMIT;








END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `Banck_Sheet_Tigare_insert`;
DELIMITER //
CREATE PROCEDURE `Banck_Sheet_Tigare_insert`()
BEGIN
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK;  END;
START TRANSACTION;


INSERT INTO `Banck_Sheet_Tigare`
           (
           `inserdate`
           ,`Exret`
           ,`Notes`
           ,`code`
           ,`ACCAcount`
           ,`UeserImort`)


           SELECT A.`inserdate` , A.`Exret` ,A.`Notes` , A.`code` ,A.`ACCAcount`  ,A.UeserImort   FROM tvp_TYPE AS A;

COMMIT;








END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `CATEGORYTYPESTB_Insert`;
DELIMITER //
CREATE PROCEDURE `CATEGORYTYPESTB_Insert`(IN `p_ID` INT, IN `p_TypeNo` TINYINT UNSIGNED, IN `p_RateType` TINYINT UNSIGNED, IN `p_CountryID` INT, IN `p_TransType` TINYINT UNSIGNED, IN `p_IsActive` TINYINT(1), IN `p_IsUpdate` TINYINT UNSIGNED, INOUT `p_MSGSTatues` INT, INOUT `p_MsgBox` LONGTEXT)
proc: BEGIN
DECLARE v_AccountID BIGINT;
DECLARE v_AccParent DECIMAL(18, 0);
DECLARE v_BName LONGTEXT;
DECLARE v_AccID BIGINT;
DECLARE v_OldeCustName LONGTEXT;
DECLARE v_OldBranchID INT;
DECLARE v_AccCode BIGINT;
DECLARE v_IDcode BIGINT;
DECLARE v_IsExist INT;
DECLARE v_CheckName LONGTEXT;
DECLARE v_ValFrom DECIMAL(12, 3);
DECLARE v_ValTo DECIMAL(12, 3);
DECLARE v_DisVal DECIMAL(12, 3);
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_MSGSTatues = 0; END;


    START TRANSACTION;
    IF p_IsUpdate = 0
       THEN
        SELECT COUNT(C.TransType) INTO v_IsExist FROM
                CATEGORYTYPESTB AS C
        WHERE
                c.CountryID = p_CountryID
                AND c.TransType = p_TransType;
        IF v_IsExist > 0
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = 'نوع التحويل لهذه الدولة موجود مسبقاً';
            ROLLBACK;
            LEAVE proc;
          END IF;
        INSERT INTO CATEGORYTYPESTB
              (
                TypeNo,
                RateType,
                CountryID,
                TransType
              )
            SELECT
                    p_TypeNo,
                    p_RateType,
                    p_CountryID,
                    p_TransType;


        INSERT INTO CATEGORYTYPESDETAILSTB
              (
                CATID,
                ValFrom,
                ValTo,
                DisVal,
                TransType,
                RateType
              )
            SELECT
                    p_ID,
                    a.ValFrom,
                    a.ValTo,
                    a.DisVal,
                    a.TransType,
                    p_RateType
            FROM
                    tvp_Type AS a;


      END IF;
    IF p_IsUpdate = 1
       THEN











        UPDATE CATEGORYTYPESDETAILSTB, tvp_Type AS a SET ValFrom = a.ValFrom,
                ValTo = a.ValTo,
                DisVal = a.DisVal,
                RateType = p_RateType WHERE
          ID = p_ID;
      END IF;


    IF p_IsUpdate = 2
       THEN


        INSERT INTO CATEGORYTYPESDETAILSTB
              (
                CATID,
                ValFrom,
                ValTo,
                DisVal,
                TransType,
                RateType
              )
            SELECT
                    p_ID,
                    a.ValFrom,
                    a.ValTo,
                    a.DisVal,
                    p_TransType,
                    p_RateType
            FROM
                    tvp_Type AS a;

      END IF;

    COMMIT;
    SET p_MSGSTatues = 1;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `CurrencyPricesOwnTb_Insert`;
DELIMITER //
CREATE PROCEDURE `CurrencyPricesOwnTb_Insert`(IN `p_ID` BIGINT, IN `p_CurrencyPower` TINYINT(1), IN `p_IsUpdate` TINYINT(1), IN `p_CurrencyIDFrom` INT, IN `p_ADDueser` INT, IN `p_IsAgent` TINYINT(1), IN `p_AgentID` INT)
BEGIN
START TRANSACTION;

  IF p_IsUpdate = 0
     THEN
      INSERT INTO CurrencyPricesOwnTb
            (
              InsertDate,
              InsertTime,
              IsActive,
              CurrencyIDFrom,
              ADDueser,
              IsAgent,
              AgentID
            )
          SELECT
                  NOW(),
                  DATE_FORMAT(NOW(), '%H:%i:%s'),
                  1,
                  p_CurrencyIDFrom,
                  p_ADDueser,
                  p_IsAgent,
                  p_AgentID;
      INSERT INTO CurrencyPriceOwnDetailsTb
            (
              CPID,
              CurrencyIDFrom,
              CurrencyIDTo,
              SalePrice,
              BuyPrice,
              CurrencyPower,
              IsActive
            )
          SELECT
                  a.CPID,
                  a.CurrencyIDFrom,
                  a.CurrencyIDTo,
                  a.SalePrice,
                  a.BuyPrice,
                  CurrencyPower,
                  1

          FROM
                  tvp_TypeTb AS a;
    END IF;

  IF p_IsUpdate = 1
     THEN
      UPDATE
              CurrencyPricesOwnTb
      SET
              IsActive = 1, InsertDate = NOW(), InsertTime = DATE_FORMAT(NOW(), '%H:%i:%s'), CurrencyIDFrom = p_CurrencyIDFrom, ADDueser = p_ADDueser, IsAgent = p_IsAgent, AgentID = p_AgentID
      WHERE
        ID = p_ID;

      DELETE FROM CurrencyPriceOwnDetailsTb WHERE
              CPID = p_ID;

      INSERT INTO CurrencyPriceOwnDetailsTb
            (
              CPID,
              CurrencyIDFrom,
              CurrencyIDTo,
              SalePrice,
              BuyPrice,
              CurrencyPower,
              IsActive
            )
          SELECT
                  a.CPID,
                  a.CurrencyIDFrom,
                  a.CurrencyIDTo,
                  a.SalePrice,
                  a.BuyPrice,
                  CurrencyPower,
                  1
          FROM
                  tvp_TypeTb AS a;
    END IF;

  COMMIT;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `CurrencyPricesTb_Insert`;
DELIMITER //
CREATE PROCEDURE `CurrencyPricesTb_Insert`(IN `p_ID` BIGINT, IN `p_CurrencyPower` TINYINT(1), IN `p_IsUpdate` TINYINT(1), IN `p_CurrencyIDFrom` INT, IN `p_ADDueser` INT)
BEGIN
START TRANSACTION;
  IF p_IsUpdate = 0
     THEN
      INSERT INTO CurrencyPricesTb
            (
              insertDate,
              InsertTime,
              ISactive,
              CurrencyIDFrom,
              ADDueser
            )
          SELECT
                  NOW(),
                  DATE_FORMAT(NOW(), '%H:%i:%s'),
                  1,
                  p_CurrencyIDFrom,
                  p_ADDueser;
      INSERT INTO CurrencyPriceDetailsTb
            (
              CPID,
              CurrencyIDFrom,
              CurrencyIDTo,
              SalePrice,
              BuyPrice,
              CurrencyPower,
              ISactive

            )
          SELECT
                  a.CPID,
                  a.CurrencyIDFrom,
                  a.CurrencyIDTo,
                  a.SalePrice,
                  a.BuyPrice,
                  CurrencyPower,
                  1

          FROM
                  tvp_TypeTb AS a;
    END IF;

  IF p_IsUpdate = 1
     THEN
      UPDATE
              CurrencyPricesTb
      SET
              ISactive = 1
      WHERE
        ID = p_ID;

      DELETE FROM CurrencyPriceDetailsTb WHERE
              CPID = p_ID;

      INSERT INTO CurrencyPriceDetailsTb
            (
              CPID,
              CurrencyIDFrom,
              CurrencyIDTo,
              SalePrice,
              BuyPrice,
              CurrencyPower,
              ISactive
            )
          SELECT
                  a.CPID,
                  a.CurrencyIDFrom,
                  a.CurrencyIDTo,
                  a.SalePrice,
                  a.BuyPrice,
                  CurrencyPower,
                  1
          FROM
                  tvp_TypeTb AS a;

      UPDATE CurrencyPriceDetailsTb
          JOIN
            tvp_TypeTb AS b
              ON CurrencyPriceDetailsTb.CPID = b.CPID SET CurrencyIDFrom = b.CurrencyIDFrom,
              CurrencyIDTo = b.CurrencyIDTo,
              SalePrice = b.SalePrice,
              BuyPrice = b.BuyPrice;
    END IF;

  COMMIT;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `DiscountsAndBounses_Insert`;
DELIMITER //
CREATE PROCEDURE `DiscountsAndBounses_Insert`()
BEGIN


    INSERT INTO DiscountValTb
          (
            InsertDate,
            EMPID,
            DiscountTypeID,
            DISVAL,
            Code,
            IsActive,
            Notes,
            IsOpened
          )

        SELECT
                NOW(),
                a.EmplloyID,
                a.Type,
                a.DisVal,
                CONCAT(CAST(a.Branch AS CHAR), '11', '-', CAST(DiscountValTb_GetMaxCode()+(ROW_NUMBER() OVER (ORDER BY a.BonesOrDis)-1) AS CHAR)),
                1,
                a.NotesDe,
                1
        FROM
                tvp_Type AS a
        WHERE
                a.BonesOrDis = 0;

    INSERT INTO IncreaseValTb
          (
            Code,
            InsertDate,
            IncreaseTypeID,
            EMPID,
            INCVAL,
            IsActive,
            IsConstant,
            Notes,
            IsOpened
          )

        SELECT
                CONCAT(CAST(a.Branch AS CHAR), '36', '-', CAST(IncreaseValTb_GetMaxCode()+(ROW_NUMBER() OVER (ORDER BY a.BonesOrDis)-1) AS CHAR)),
                NOW(),
                a.Type,
                a.EmplloyID,
                a.BounsVal,
                1,
                0,
                a.NotesDe,
                1
        FROM
                tvp_Type AS a
        WHERE
                a.BonesOrDis = 1;

  END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `insertprofilename`;
DELIMITER //
CREATE PROCEDURE `insertprofilename`(IN `p_IDPROFILE` INT, IN `p_NAMEPROFILE` LONGTEXT, IN `p_OperationType` TINYINT(1), INOUT `p_msg` LONGTEXT, INOUT `p_MSGStat` INT)
proc: BEGIN
DECLARE v_idcode INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; SET p_MSGStat = 0; CALL ERROR_PROC(); END;
      START TRANSACTION;

      IF p_OperationType = 0
         THEN



          SELECT `ProfID` INTO v_idcode FROM
                  `ProfileName`
          WHERE
                  `ProfID` = p_IDPROFILE;

          IF v_idcode = p_IDPROFILE
             THEN
              SET p_MSGStat = 0;
              SET p_msg = 'عذراً هذا الرقم موجود مسبقاً';
              ROLLBACK;
              LEAVE proc;
            END IF;


          INSERT INTO `ProfileName`
                (
                  `ProfID`,
                  `ProfileName`
                )
          VALUES
                  (
                    p_IDPROFILE, p_NAMEPROFILE
                  );

          INSERT INTO `UserROOLMAINPROFILETP`
                (
                  `profid`,
                  `canshow`,
                  `mainid`
                )
              SELECT
                      p_IDPROFILE,
                      a.`canshow`,
                      a.`mainid`
              FROM
                      tvp_TypeUserROOLMAIN AS a;

          INSERT INTO `Ueser_Group_main`
                (
                  `Profile`,
                  `GrouID`,
                  `Canshow`,
                  `ManID`
                )
              SELECT
                      p_IDPROFILE,
                      `GrouID`,
                      `Canshow`,
                      `ManID`
              FROM
                      tvp_Ueser_Group_main_insertType AS a;

          INSERT INTO `UserAccessProfileTemplate`
                (
                  `ProfileID`,
                  `MainID`,
                  `Group_ID`,
                  `ScreenID`,
                  `CanShow`,
                  `CanSave`,
                  `CanEdit`,
                  `CanDelete`,
                  `CanSearch`,
                  `CanPrint`
                )
              SELECT
                      p_IDPROFILE,
                      a.`MainID`,
                      Group_ID,
                      `ScreenID`,
                      `CanShow`,
                      `CanSave`,
                      `CanEdit`,
                      `CanDelete`,
                      `CanSearch`,
                      `CanPrint`
              FROM
                      tvp_UserAccessProfileTemplate_type AS a;


          INSERT INTO `NotificationPROFILE`
                (
                  `profileID`,
                  `NotificationID`,
                  `canshow`
                )
              SELECT
                      a.ProfID,
                      b.NotificationID,
                      1
              FROM
                      ProfileName AS a
                  CROSS JOIN
                    frmNotifications AS b
              WHERE
                      ProfID = p_IDPROFILE
              ORDER BY
                      a.ProfID,
                      b.NotificationID ASC;



         ELSEIF  p_OperationType = 1
         THEN

          UPDATE
                  `ProfileName`
          SET
                  `ProfileName` = p_NAMEPROFILE
          WHERE
            `ProfID` = p_IDPROFILE;

          DELETE FROM
          `UserROOLMAINPROFILETP`
          WHERE
                  `profid` = p_IDPROFILE;
          INSERT INTO `UserROOLMAINPROFILETP`
                (
                  `profid`,
                  `canshow`,
                  `mainid`
                )
              SELECT
                      p_IDPROFILE,
                      a.`canshow`,
                      a.`mainid`
              FROM
                      tvp_TypeUserROOLMAIN AS a;

          DELETE FROM
          `Ueser_Group_main`
          WHERE
                  `Profile` = p_IDPROFILE;
          INSERT INTO `Ueser_Group_main`
                (
                  `Profile`,
                  `GrouID`,
                  `Canshow`,
                  `ManID`
                )
              SELECT
                      p_IDPROFILE,
                      `GrouID`,
                      `Canshow`,
                      `ManID`
              FROM
                      tvp_Ueser_Group_main_insertType AS a;

          DELETE FROM
          `UserAccessProfileTemplate`
          WHERE
                  `ProfileID` = p_IDPROFILE;
          INSERT INTO `UserAccessProfileTemplate`
                (
                  `ProfileID`,
                  `MainID`,
                  `Group_ID`,
                  `ScreenID`,
                  `CanShow`,
                  `CanSave`,
                  `CanEdit`,
                  `CanDelete`,
                  `CanSearch`,
                  `CanPrint`
                )
              SELECT
                      p_IDPROFILE,
                      a.`MainID`,
                      Group_ID,
                      `ScreenID`,
                      `CanShow`,
                      `CanSave`,
                      `CanEdit`,
                      `CanDelete`,
                      `CanSearch`,
                      `CanPrint`
              FROM
                      tvp_UserAccessProfileTemplate_type AS a;
        END IF;

      COMMIT;
      SET p_MSGStat = 1;
  END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `MultiAcountEditTB_Insert`;
DELIMITER //
CREATE PROCEDURE `MultiAcountEditTB_Insert`(IN `p_Code` LONGTEXT, IN `p_BranchID` INT, IN `p_BranchIDTo` INT, IN `p_SafeID` BIGINT, IN `p_CurrencyID` INT, IN `p_IsUpdate` TINYINT(1), INOUT `p_MSGSTatues` INT, INOUT `p_MsgBox` LONGTEXT, IN `p_MovmentType` LONGTEXT, IN `p_OverAllVal` DECIMAL(18,3), IN `p_ValType` INT, IN `p_FirstAccID` BIGINT)
proc: BEGIN
DECLARE v_IDCode         INT;
DECLARE v_SumVal         DECIMAL(18, 3);
DECLARE v_MainBranchID   INT;
DECLARE v_MainCurrentAcc BIGINT;
DECLARE v_TotalDebit DECIMAL(18, 3) DEFAULT 0;
DECLARE v_TotalCredit DECIMAL(18, 3) DEFAULT 0;
DECLARE v_MultiAccAccID  BIGINT;
DECLARE v_FirstDebit DECIMAL(18, 3) DEFAULT 0;
DECLARE v_FirstCredit DECIMAL(18, 3) DEFAULT 0;
DECLARE v_b      INT;
DECLARE v_bCount INT;
DECLARE v_BalBranchID INT;
DECLARE v_Net         DECIMAL(18, 3);
DECLARE v_CurrentAcc BIGINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;



      START TRANSACTION;



      SELECT ID, IFNULL(CurrentAccID, 0) INTO v_MainBranchID, v_MainCurrentAcc FROM
              CoBranch
      WHERE
              IsMain = 1;
      SELECT AccID INTO v_MultiAccAccID FROM
              AccountsTb a
      WHERE
              a.AccParent = 1030401
              AND a.BranchID = p_BranchIDTo;

      IF v_MainBranchID IS NULL
         THEN
          SET p_MSGSTatues = 0;
          SET p_MsgBox = N'لم يتم تحديد فرع رئيسي في النظام.';
          ROLLBACK;
          LEAVE proc;
        END IF;


      SELECT IFNULL(MAX(IDCode), 0) + 1 INTO v_IDCode FROM
              MultiAcountEditTB;


      SELECT CASE                                WHEN p_ValType = 0                                  THEN                                  IFNULL(SUM(Credit), 0)                                ELSE                                IFNULL(SUM(Debit), 0)                        END INTO v_SumVal FROM
              tvp_Type;

      IF v_SumVal <> p_OverAllVal
         THEN
          SET p_MSGSTatues = 0;
          SET p_MsgBox = N'مجموع الطرف الثاني لا يساوي القيمة الإجمالية';
          ROLLBACK;
          LEAVE proc;
        END IF;


      INSERT INTO MultiAcountEditTB
            (
              Code,
              InsertDate,
              BranchID,
              SafeID,
              CurrencyID,
              IsActive,
              IDCode,
              MovmentType
            )
      VALUES
              (
                p_Code, NOW(), p_BranchID, p_SafeID, p_CurrencyID, 1, v_IDCode, p_MovmentType
              );




      DROP TEMPORARY TABLE IF EXISTS tmp_AllEntries;
CREATE TEMPORARY TABLE tmp_AllEntries (
          RowID      INT AUTO_INCREMENT PRIMARY KEY,
          BranchID   INT,
          AccID      BIGINT,
          Debit      DECIMAL(18, 3),
          Credit     DECIMAL(18, 3),
          IsFirst    BIT,
          AccIDToRef BIGINT
        );


      IF p_ValType = 1
         THEN
          SET v_FirstDebit = 0;
          SET v_FirstCredit = p_OverAllVal;
         ELSE
          SET v_FirstDebit = p_OverAllVal;
          SET v_FirstCredit = 0;
        END IF;

      INSERT INTO tmp_AllEntries
            (
              BranchID,
              AccID,
              Debit,
              Credit,
              IsFirst,
              AccIDToRef
            )
      VALUES
              (
                p_BranchID, p_FirstAccID, v_FirstDebit, v_FirstCredit, 1, v_MultiAccAccID
              );


      INSERT INTO tmp_AllEntries
            (
              BranchID,
              AccID,
              Debit,
              Credit,
              IsFirst,
              AccIDToRef
            )
          SELECT
                  a.BranchIDTo,
                  a.AccIDTo,
                  CASE                           WHEN p_ValType = 1                             THEN                             a.Debit                           ELSE                           0                   END,
                  CASE                           WHEN p_ValType = 0                             THEN                             a.Credit                           ELSE                           0                   END,
                  0,
                  CASE                           WHEN a.BranchIDTo = p_BranchID                             THEN                             p_FirstAccID                           ELSE                           v_MainCurrentAcc                   END
          FROM
                  tvp_Type a;




      INSERT INTO ExSyAccounts2026.AccSafeActivityTb
            (
              SafeID,
              Debit,
              Credit,
              InsertDate,
              ISID,
              IsActive,
              TypeID,
              OperationTypeID,
              AccBranchID,
              AccIDFrom,
              AccIDTo
            )
          SELECT
                  p_SafeID,
                  Debit,
                  Credit,
                  NOW(),
                  p_Code,
                  1,
                  55,
                  91,
                  BranchID,
                  AccID,
                  AccIDToRef
          FROM
                  tmp_AllEntries;




      DROP TEMPORARY TABLE IF EXISTS tmp_BranchBalances;
CREATE TEMPORARY TABLE tmp_BranchBalances (
          BranchID  INT,
          NetAmount DECIMAL(18, 3)
        );

      INSERT INTO tmp_BranchBalances
            (
              BranchID,
              NetAmount
            )
          SELECT
                  BranchID,
                  SUM(Debit - Credit) AS NetAmount
          FROM
                  tmp_AllEntries
          WHERE
                  BranchID <> v_MainBranchID
          GROUP BY
                  BranchID;




      SET v_b = 1;
      SET v_bCount = (SELECT COUNT(*) FROM tmp_BranchBalances);

      WHILE v_b <= v_bCount
         DO
          SELECT BranchID, NetAmount INTO v_BalBranchID, v_Net FROM
                  (   SELECT
                              ROW_NUMBER() OVER (ORDER BY BranchID) AS rn,
                              BranchID,
                              NetAmount
                      FROM
                              tmp_BranchBalances) t
          WHERE
                  rn = v_b;

          SET v_CurrentAcc = IFNULL((SELECT CurrentAccID FROM CoBranch WHERE ID = v_BalBranchID) , 0);

          IF v_Net > 0
             THEN


              INSERT INTO ExSyAccounts2026.AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo
                    )
              VALUES
                      (
                        p_SafeID, 0, v_Net, NOW(), p_Code, 1, 55, 91, v_BalBranchID, v_MainCurrentAcc, v_CurrentAcc
                      );


              INSERT INTO ExSyAccounts2026.AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo
                    )
              VALUES
                      (
                        p_SafeID, v_Net, 0, NOW(), p_Code, 1, 55, 91, v_MainBranchID, v_CurrentAcc, v_MainCurrentAcc
                      );
             ELSEIF  v_Net < 0
             THEN

              SET v_Net = -v_Net;

              INSERT INTO ExSyAccounts2026.AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo
                    )
              VALUES
                      (
                        p_SafeID, v_Net, 0, NOW(), p_Code, 1, 55, 91, v_BalBranchID, v_MainCurrentAcc, v_CurrentAcc
                      );


              INSERT INTO ExSyAccounts2026.AccSafeActivityTb
                    (
                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo
                    )
              VALUES
                      (
                        p_SafeID, 0, v_Net, NOW(), p_Code, 1, 55, 91, v_MainBranchID, v_CurrentAcc, v_MainCurrentAcc
                      );
            END IF;

          SET v_b = v_b + 1;
        END WHILE;




      INSERT INTO MultiAcountEditDetailsTB
            (
              MISID,
              AccID,
              Debit,
              Credit,
              Notes,
              IsActive,
              InsertDate,
              Branch,
              AccIDTo,
              BranchIDTo
            )
          SELECT
                  p_Code,
                  p_FirstAccID,
                  CASE                           WHEN p_ValType = 1                             THEN                             a.Debit                           ELSE                           0                   END,
                  CASE                           WHEN p_ValType = 0                             THEN                             a.Credit                           ELSE                           0                   END,
                  IFNULL(a.NotesDe, N'تفاصيل القيد'),
                  1,
                  NOW(),
                  a.Branch,
                  a.AccIDTo,
                  a.BranchIDTo
          FROM
                  tvp_Type a;




      SELECT SUM(IFNULL(Debit, 0)), SUM(IFNULL(Credit, 0)) INTO v_TotalDebit, v_TotalCredit FROM
              ExSyAccounts2026.AccSafeActivityTb
      WHERE
              ISID = p_Code;

      IF ABS(v_TotalDebit - v_TotalCredit) > 0.01
         THEN
          SET p_MSGSTatues = 0;
          SET p_MsgBox = CONCAT(N'خطأ في التوازن المحاسبي. المدين: ', CAST(v_TotalDebit AS CHAR), N'، الدائن: ', CAST(v_TotalCredit AS CHAR));
          ROLLBACK;
          LEAVE proc;
        END IF;

      COMMIT;
      SET p_MSGSTatues = 1;
      SET p_MsgBox = N'تم حفظ القيد متعدد الحسابات بنجاح.';



  END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `NewCurrencyBuyandSale_Insert`;
DELIMITER //
CREATE PROCEDURE `NewCurrencyBuyandSale_Insert`(IN `p_IDcode` INT, IN `p_Code` LONGTEXT, IN `p_BranchID` INT, IN `p_FirstCurrency` INT, IN `p_BPrice1` DOUBLE, IN `p_CurrencyTo` INT, IN `p_BPrice2` DOUBLE, IN `p_Notes` LONGTEXT, IN `p_UeserInset` INT, INOUT `p_MSGSTatues` INT, INOUT `p_MsgBox` LONGTEXT, IN `p_IsCachorBank` INT, IN `p_SAFTypeform` BIGINT, IN `p_SaFACCount` BIGINT, IN `p_SafTypeTo` BIGINT, IN `p_SAFFACCTO` BIGINT, IN `p_Purchaseprice` DECIMAL(13,3), IN `p_CusIDNo` VARCHAR(50), IN `p_Phone` VARCHAR(25), IN `p_thepurpose` INT, IN `p_CustName` VARCHAR(50), IN `p_BankID` INT, IN `p_AccAcountBank` VARCHAR(50), IN `p_CHECKTYPE` INT, IN `p_AccountOwnName` LONGTEXT, IN `p_Typeoftransaction` INT, IN `p_Transactionnumber` BIGINT, IN `p_AgentBankNumber` LONGTEXT, IN `p_CountryID` INT, IN `p_PrType` INT)
proc: BEGIN
DECLARE v_GETID INT;
DECLARE v_ACCaountDR BIGINT;
DECLARE v_IsDefalut BIT;
DECLARE v_NatNumber VARCHAR(50);
DECLARE v_CusName LONGTEXT;
DECLARE v_DEbitValue DECIMAL(13, 6);
DECLARE v_BuyPrice DECIMAL(13, 6);
DECLARE v_ACCRevenueCalculation BIGINT;
DECLARE v_valueevenueCalculation DECIMAL(13, 3);
DECLARE v_IsEnabled BIT;
DECLARE v_MaxVal DECIMAL(15, 3);
DECLARE v_CustCurrVal DECIMAL(15, 3);
DECLARE v_ValCanSale DECIMAL(15, 3);
DECLARE v_FatherPerintACC BIGINT;
DECLARE v_FAtherPERints BIGINT;
DECLARE v_FirstCurrencyd BIGINT;
DECLARE v_FirstCurrencysASSS BIGINT;
DECLARE v_ACCFPrintCridet BIGINT;
DECLARE v_ACCFPrintCridetACC BIGINT;
DECLARE v_SAFACCAS BIGINT;
DECLARE v_AccTo BIGINT;
DECLARE v_CanDepit BIT;
DECLARE v_EMPORCUST_G DECIMAL(15, 3);
DECLARE v_MovementType LONGTEXT;
DECLARE v_SafeIDMovement LONGTEXT;
DECLARE v_BankAccID INT;
DECLARE v_getDate DATE DEFAULT NOW();
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_MSGSTatues = 0; END;

      START TRANSACTION;

      SELECT bt.ACCID INTO v_BankAccID FROM
              BBranchTb bt
      WHERE
              (       bt.ACCID = p_BankID
                      OR bt.ACCID = 0);
      SELECT CMT.IsDefault INTO v_IsDefalut FROM
              CurrencyMainTb AS CMT
      WHERE
              CMT.ID = p_CurrencyTo
 ORDER BY CMT.ID DESC LIMIT 1;
      SELECT IFNULL(IDcode, 0) INTO v_GETID FROM
              CurrenciesBuyandsellTB AS a
      WHERE
              a.IDcode = p_IDcode
              AND a.TypeOP = 25;
      IF v_GETID = p_IDcode
         THEN
          SET p_MSGSTatues = 2;
          SET p_MsgBox = 'عذرا هذه الرقم موجود مسبقا الرجاء المحاولة في وقت لاحق ';
          ROLLBACK;
          LEAVE proc;
        END IF;


      IF p_IsCachorBank = 0
         THEN
          SELECT CASE                                      WHEN a.CurrencyPower = 1                                        THEN                                        a.Costof                                      ELSE                                      1 / a.Costof                              END INTO v_BuyPrice FROM
                  (SELECT
					z.CuName,
					z.Costof,

					z.Foreignbalance,
					z.LocalBalance,
					z.costmerForPreseEn,
					z.SafeTransfer,

					IFNULL(z.costmerForPreseEn + z.Foreignbalance+z.SafeTransfer, 0.000)		   AS TotlaPrasceFor_Currency,
					IFNULL(LocalBalance + (Costof * costmerForPreseEn), 0.000) AS TotalLocalBalance,
					z.ID,
          CurrencyPower
			FROM
					(   SELECT
								z.CuName,
								IFNULL((SELECT
											CASE 													WHEN c.CurrencyPower = 0 														THEN 														CASE 																WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																	THEN 																	0.00 																ELSE 																(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) / (SUM(CRedetDL) - SUM(ACC_DEPET_DL)) 														END 													ELSE 													CASE 															WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																THEN 																0.00 															ELSE 															(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) / (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) 													END 											END AS Costof
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
              NewCurrencyPriceOwnDetailsTb AS c
                ON b.ID = c.CurrencyIDTo
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower), 0.00) AS Costof,

								IFNULL((SELECT
											(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) AS Foreignbalance
									FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID


									GROUP BY
											a.ACCFRom), 0.000) AS Foreignbalance,


								IFNULL((SELECT
											(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) AS LocalBalance
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS a
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID

									GROUP BY
											a.ACCFRom), 0.000) AS LocalBalance,
								IFNULL((SELECT
											SUM(x.Credit) - SUM(x.Debit)
									 FROM
											ExSyAccountsCurrency_AccSafeActivityTb AS x
											left join TB_Users as b on x.AccIDFrom=accid
									WHERE

												 x.OperationTypeID=40

											AND x.CurrencyID = z.ID

											AND AccIDTo<>b.AccID
											AND x.AccBranchID=(p_BranchID)

											), 0.000) AS costmerForPreseEn,

											IFNULL((SELECT
											SUM(x.Credit) - SUM(x.Debit)
									 FROM
											ExSyAccountsCurrency_AccSafeActivityTb AS x
									WHERE

											 x.OperationTypeID=34
											AND x.TypeID <> 1
											AND x.CurrencyID = z.ID
											AND x.AccBranchID=(p_BranchID)

											), 0.000) AS SafeTransfer,
								(SELECT c.CurrencyPower
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
              NewCurrencyPriceOwnDetailsTb AS c
                ON b.ID = c.CurrencyIDTo
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower) AS CurrencyPower,

								z.ID
						FROM
								CurrencyMainTb AS z
						WHERE
								z.ID <> 1) AS z) AS a
          WHERE
                  a.ID = p_FirstCurrency;
        END IF;

      IF p_IsCachorBank = 1
         THEN
          SELECT CASE                                      WHEN a.CurrencyPower = 1                                        THEN                                        a.Costof                                      ELSE                                      1 / a.Costof                              END INTO v_BuyPrice FROM
                  (SELECT
					z.CuName,
					z.Costof,

					z.Foreignbalance,
					z.LocalBalance,
					z.costmerForPreseEn,
					z.SafeTransfer,

					IFNULL(z.costmerForPreseEn + z.Foreignbalance+z.SafeTransfer, 0.000)		   AS TotlaPrasceFor_Currency,
					IFNULL(LocalBalance + (Costof * costmerForPreseEn), 0.000) AS TotalLocalBalance,
					z.ID,
          z.CurrencyPower
			FROM
					(   SELECT
								z.CuName,
								IFNULL((SELECT
											CASE 													WHEN c.CurrencyPower = 0 														THEN 														CASE 																WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																	THEN 																	0.00 																ELSE 																(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) / (SUM(CRedetDL) - SUM(ACC_DEPET_DL)) 														END 													ELSE 													CASE 															WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																THEN 																0.00 															ELSE 															(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) / (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) 													END 											END AS Costof
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
											CurrencyPriceDetailsTb AS c
												ON b.ID = c.CurrencyIDTo
									WHERE
											a.ISactive = 1
											AND (a.TYPEFROM = 1 OR a.TYPEFROM = 2)
											AND a.CountryID=(p_CountryID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower), 0.00) AS Costof,

								IFNULL((SELECT
											(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) AS Foreignbalance
									FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
									WHERE
											a.ISactive = 1
											AND (a.TYPEFROM = 1 OR a.TYPEFROM = 2)
											AND a.CountryID=(p_CountryID)
											AND a.ACCFRom = z.ID


									GROUP BY
											a.ACCFRom), 0.000) AS Foreignbalance,


								IFNULL((SELECT
											(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) AS LocalBalance
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS a
									WHERE
											a.ISactive = 1
											AND (a.TYPEFROM = 1 OR a.TYPEFROM = 2)
											AND a.CountryID=(p_CountryID)
											AND a.ACCFRom = z.ID

									GROUP BY
											a.ACCFRom), 0.000) AS LocalBalance,
							0.000 AS costmerForPreseEn,

				0.000 AS SafeTransfer,
								(SELECT c.CurrencyPower
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
											CurrencyPriceDetailsTb AS c
												ON b.ID = c.CurrencyIDTo
									WHERE
											a.ISactive = 1
											AND (a.TYPEFROM = 1 OR a.TYPEFROM = 2)
											AND a.CountryID=(p_CountryID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower) AS CurrencyPower,

								z.ID
						FROM
								CurrencyMainTb AS z
						WHERE
								z.ID <> 1) AS z) AS a
          WHERE
                  a.ID = p_FirstCurrency;
        END IF;
      SELECT IFNULL(SUM(c.BPrice1), 0.000) INTO v_CustCurrVal FROM
              CurrenciesBuyandsellTB c
      WHERE
              c.FirstCurrency = p_FirstCurrency
              AND c.CustIDNo = p_CusIDNo
              AND c.TypeOP = 25;

      SELECT c.NatNumber, c.CustName INTO v_NatNumber, v_CusName FROM
              CustomersTb c
      WHERE
              c.ACCID = p_SaFACCount;

      IF v_NatNumber IS NOT NULL
         THEN
          SET p_CusIDNo = v_NatNumber;
          SET p_CustName = v_CusName;
        END IF;
      INSERT INTO `CurrenciesBuyandsellTB`
            (
              `IDcode`,
              `Code`,
              `BranchID`,
              `SaFACCount`,
              `AccSafeTo`,
              `FirstCurrency`,
              `BPrice1`,
              `CurrencyTo`,
              `BPrice2`,
              `Notes`,
              `TotalQt`,
              TotalPriceByLD,
              `UeserInset`,
              `ISactive`,
              `countUpdate`,
              TypeOP,
              Purchasingprice,
              IsCachorBank,
              CustIDNo,
              Phone,
              thepurpose,
              CusName,
              SafeTypefrom,
              SafeTypeTo,
              BankID,
              AccAcountBank,
              CheckType,
              AccountOwnName,
              Typeoftransaction,
              Transactionnumber,
              AgentBankNumber,
              CountryID
            )
      VALUES
              (
                p_IDcode, p_Code, p_BranchID, p_SaFACCount, p_SAFFACCTO, p_FirstCurrency, p_BPrice1, p_CurrencyTo, p_BPrice2, p_Notes, 1, 0, p_UeserInset, 1, 0, 25, p_Purchaseprice, p_IsCachorBank, p_CusIDNo, p_Phone, p_thepurpose, p_CustName, p_SAFTypeform, p_SafTypeTo, p_BankID, p_AccAcountBank, p_CHECKTYPE, p_AccountOwnName, p_Typeoftransaction, p_Transactionnumber, p_AgentBankNumber, p_CountryID
              );


      SELECT a.FatherPerintACC, a.EMFtherPRint, a.ACCFPrintCridet INTO v_FatherPerintACC, v_FAtherPERints, v_ACCFPrintCridet FROM
              CurrencyMainTb AS a
      WHERE
              a.ID = p_FirstCurrency
 ORDER BY a.ID DESC LIMIT 1;
      IF p_IsCachorBank = 0
        OR
        p_IsCachorBank = 1
         THEN

          IF p_IsCachorBank = 0
             THEN
              SELECT ACCID INTO v_FirstCurrencysASSS FROM
                      AccountsTb AS a
              WHERE
                      a.AccParent = v_FatherPerintACC
                      AND a.BranchID = p_BranchID;
            END IF;

          IF p_IsCachorBank = 1
             THEN
              SELECT ACCID INTO v_FirstCurrencysASSS FROM
                      AccountsTb AS a
              WHERE
                      a.AccParent LIKE '101012%'
                      AND a.CountryID = p_CountryID
                      AND a.AccCurID = p_FirstCurrency;
            END IF;
          SELECT ACCID INTO v_FirstCurrencyd FROM
                  AccountsTb AS a
          WHERE
                  a.AccParent = v_FAtherPERints
                  AND a.BranchID = p_BranchID;
          SELECT ACCID INTO v_ACCFPrintCridetACC FROM
                  AccountsTb AS a
          WHERE
                  a.AccParent = v_ACCFPrintCridet
                  AND a.BranchID = p_BranchID
                  AND a.AccActive = 1;
          SELECT ACCID INTO v_SAFACCAS FROM
                  TB_Users AS a
          WHERE
                  a.USID = p_UeserInset
 ORDER BY a.USID DESC LIMIT 1;


          SET v_AccTo = p_SAFFACCTO;
          IF p_SAFTypeform <> 0
             THEN
SET v_AccTo = p_SaFACCount;
            END IF;

          IF v_IsDefalut = 1
             THEN


              IF p_SAFTypeform = 0
                 THEN
SET v_SAFACCAS = p_SaFACCount;
SET v_EMPORCUST_G = `BranchCur_GetAccVal_SAfs`(p_SaFACCount, p_FirstCurrency, p_BranchID);
                  IF p_BPrice1 > v_EMPORCUST_G
                     THEN

                      SET p_MsgBox = 'عذرا الشركة لايمكنها تغطية هذه القيمة حاليا الرجاء التواصل مع الإدارة بالخصوص ';
                      SET p_MSGSTatues = 0;
                      ROLLBACK;
                      LEAVE proc;
                    END IF;
                END IF;
              IF p_PrType <> 1
                 THEN
                  IF p_SAFTypeform <> 0
                     THEN
                      SELECT ACCID INTO v_SAFACCAS FROM
                              TB_Users AS a
                      WHERE
                              a.USID = p_UeserInset
 ORDER BY a.USID DESC LIMIT 1;
SET v_EMPORCUST_G = `BranchCur_GetAccVal_SAfs`(p_SaFACCount, p_FirstCurrency, p_BranchID);
                      IF p_BPrice1 > v_EMPORCUST_G
                         THEN
                          SET p_MsgBox = 'عذرا الشركة لايمكنها تغطية هذه القيمة حاليا الرجاء التواصل مع الإدارة بالخصوص ';
                          SET p_MSGSTatues = 0;
                          ROLLBACK;
                          LEAVE proc;
                        END IF;
                    END IF;







                  SELECT IFNULL(a.IsEnabled, 0), a.MaxVal INTO v_IsEnabled, v_MaxVal FROM
                          CurrLimetedTB a
                  WHERE
                          a.CurID = p_FirstCurrency;


                  IF v_IsEnabled = 1
                     THEN


                      IF p_BPrice1 > v_MaxVal
                         THEN
                          SET p_MsgBox = CONCAT('عذرا القيمة المراد بيعها أكبر من القيمة المسموح للعميل شراءها', ' ', 'وأقصى قيمة يمكن شراءوها هي', ' ', CAST(v_MaxVal AS CHAR));
                          SET p_MSGSTatues = 0;
                          ROLLBACK;
                          LEAVE proc;
                        END IF;
SET v_ValCanSale = v_CustCurrVal + p_BPrice1;
                      IF v_CustCurrVal > v_MaxVal
                         THEN
                          SET p_MsgBox = 'عذرا لقد تجاوز العميل الحد المسموح به من العملة طبقا لشروط مصرف ليبيا المركزي';
                          SET p_MSGSTatues = 0;
                          ROLLBACK;
                          LEAVE proc;
                        END IF;

                      IF v_ValCanSale > v_MaxVal
                         THEN
                          SET p_MsgBox = CONCAT('عذرا لقد تجاوز العميل الحد المسموح به من العملة طبقا لشروط مصرف ليبيا المركزي', ' ', 'وأقصى قيمة يمكنه شراءوها حاليا هي', ' ', CAST(v_MaxVal - v_CustCurrVal AS CHAR));
                          SET p_MSGSTatues = 0;
                          ROLLBACK;
                          LEAVE proc;
                        END IF;
                    END IF;
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 1, p_BPrice1, p_Purchaseprice);
                  IF p_IsCachorBank = 0
                    OR
                    p_IsCachorBank = 1
                     THEN

                      CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice1, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_SAFACCAS, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_FirstCurrency, 0, 1, v_SafeIDMovement);
SET v_DEbitValue = p_BPrice1 * v_BuyPrice;
                      CALL AccSafeActivityTb_Insert(p_UeserInset, 0, v_DEbitValue, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_FirstCurrencysASSS, p_SAFFACCTO, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
                      IF p_PrType = 1
                         THEN
                          IF p_SAFTypeform <> 0
                             THEN
                              SELECT ACCID INTO v_SAFACCAS FROM
                                      TB_Users AS a
                              WHERE
                                      a.USID = p_UeserInset
 ORDER BY a.USID DESC LIMIT 1;
SET v_EMPORCUST_G = `BranchCur_GetAccVal_SAfs`(p_SaFACCount, p_FirstCurrency, p_BranchID);
                              IF p_BPrice1 > v_EMPORCUST_G
                                 THEN
                                  SET p_MsgBox = 'عذرا الشركة لايمكنها تغطية هذه القيمة حاليا الرجاء التواصل مع الإدارة بالخصوص ';
                                  SET p_MSGSTatues = 0;
                                  ROLLBACK;
                                  LEAVE proc;
                                END IF;
                            END IF;
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 1, p_BPrice1, p_Purchaseprice);
                          CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice1, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SaFACCount, p_SAFFACCTO, 0, 0, v_SafeIDMovement, p_FirstCurrency, 0, 1, v_SafeIDMovement);
SET v_DEbitValue = p_BPrice1 * v_BuyPrice;
                          CALL AccSafeActivityTb_Insert(p_UeserInset, 0, v_DEbitValue, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SaFACCount, p_SAFFACCTO, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
                        END IF;

                      IF p_PrType <> 1
                         THEN
                          IF p_SAFTypeform <> 0
                             THEN
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 0, p_BPrice1, p_Purchaseprice);
                              CALL AccSafeActivityTb_Insert(p_UeserInset, 0, p_BPrice1, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_ACCFPrintCridetACC, p_SaFACCount, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
                              CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice1, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_FirstCurrencyd, p_SaFACCount, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
                              CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice1, 0, v_getDate, p_Notes, p_Code, 22, 22, p_BranchID, p_SaFACCount, p_SAFFACCTO, 0, 0, v_SafeIDMovement, p_FirstCurrency, 0, 1, v_SafeIDMovement);
                              CALL AccSafeActivityTb_Insert(p_UeserInset, 0, p_BPrice1, v_getDate, p_Notes, p_Code, 22, 22, p_BranchID, v_SAFACCAS, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_FirstCurrency, 0, 1, v_SafeIDMovement);
                            END IF;
                        END IF;
                      IF p_SafTypeTo = 0
                         THEN
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 1, p_BPrice1, p_Purchaseprice);
                          CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice2, 0.00, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SAFFACCTO, v_FirstCurrencysASSS, 0, 0, v_MovementType, p_CurrencyTo, 0, 1, v_SafeIDMovement);
                        END IF;
                    END IF;
                  IF p_SafTypeTo <> 0
                     THEN

                      IF p_IsCachorBank = 0
                        OR
                        p_IsCachorBank = 1
                         THEN


                          SELECT IFNULL(ct.CandDebit, 1) INTO v_CanDepit FROM
                                  CustomersTb AS ct
                          WHERE
                                  ct.ACCID = p_SAFFACCTO;
SET v_EMPORCUST_G = `EMPCUST_GetAccValCashOnly`(p_SAFFACCTO);
                          IF p_BPrice2 > v_EMPORCUST_G
                            AND
                            (v_CanDepit = 0
                            OR
                            v_CanDepit IS NULL)
                             THEN
                              SET p_MsgBox = 'عذرا رصيد العميل النقدي لا يغطي هذه العملية';
                              SET p_MSGSTatues = 0;
                              ROLLBACK;
                              LEAVE proc;
                            END IF;
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 0, p_BPrice1, p_Purchaseprice);
                          CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice2, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SAFFACCTO, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
                        END IF;

                    END IF;


                  IF p_PrType <> 1
                     THEN
                      IF p_SAFTypeform = 0
                         THEN
SET v_SAFACCAS = p_SaFACCount;
                        END IF;

                      IF p_SAFTypeform <> 0
                         THEN
                          SELECT ACCID INTO v_SAFACCAS FROM
                                  TB_Users AS a
                          WHERE
                                  a.USID = p_UeserInset
 ORDER BY a.USID DESC LIMIT 1;
                        END IF;
                    END IF;

                  IF p_PrType = 1
                     THEN
SET v_SAFACCAS = p_SaFACCount;
                    END IF;
SET v_valueevenueCalculation = p_BPrice2 - v_DEbitValue;
                  INSERT INTO ExSyAccountsCurrency2026.CurrencymovementPruse
                        (
                          `ISID`,
                          `ACCFRom`,
                          `ACCCRint0`,
                          `CRedetTO`,
                          `CRedetDL`,
                          `ACC_DEPET_DL`,
                          `ACC_DEPET_TO`,
                          `UESER_INSERt`,
                          `Inseart_Date`,
                          `ISactive`,
                          `BID`,
                          `TYPEFROM`,
                          Purchaseprice,
                          ACCID,
                          Typeofsalebuyorsell,
                          NetSale,
                          salesPurchaseprice,
                          BankID,
                          CountryID
                        )
                  VALUES
                          (
                            p_Code, p_FirstCurrency, p_CurrencyTo, 0.000, 0.00, v_DEbitValue, p_BPrice1, p_UeserInset, NOW(), 1, p_BranchID, p_IsCachorBank, v_BuyPrice, v_SAFACCAS, 1, v_valueevenueCalculation, p_Purchaseprice, p_BankID, p_CountryID
                          );

                END IF;
            END IF;
        END IF;

      IF p_IsCachorBank = 3
         THEN








          IF p_SAFTypeform = 10105
             THEN
              SELECT CASE                                          WHEN a.CurrencyPower = 1                                            THEN                                            a.Costof                                          ELSE                                          1 / a.Costof                                  END INTO v_BuyPrice FROM
                      (SELECT
					z.CuName,
					z.Costof,

					z.Foreignbalance,
					z.LocalBalance,
					z.costmerForPreseEn,
					z.SafeTransfer,

					IFNULL(z.costmerForPreseEn + z.Foreignbalance+z.SafeTransfer, 0.000)		   AS TotlaPrasceFor_Currency,
					IFNULL(LocalBalance + (Costof * costmerForPreseEn), 0.000) AS TotalLocalBalance,
					z.ID,
          CurrencyPower
			FROM
					(   SELECT
								z.CuName,
								IFNULL((SELECT
											CASE 													WHEN c.CurrencyPower = 0 														THEN 														CASE 																WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																	THEN 																	0.00 																ELSE 																(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) / (SUM(CRedetDL) - SUM(ACC_DEPET_DL)) 														END 													ELSE 													CASE 															WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																THEN 																0.00 															ELSE 															(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) / (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) 													END 											END AS Costof
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
              NewCurrencyPriceOwnDetailsTb AS c
                ON b.ID = c.CurrencyIDTo

									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 3
											AND a.BID=(p_BranchID)
                      AND a.BankID=(p_BankID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower), 0.00) AS Costof,

								IFNULL((SELECT
											(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) AS Foreignbalance
									FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 3
											AND a.BID=(p_BranchID)
                      AND a.BankID=(p_BankID)
											AND a.ACCFRom = z.ID


									GROUP BY
											a.ACCFRom), 0.000) AS Foreignbalance,


								IFNULL((SELECT
											(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) AS LocalBalance
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS a
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 3
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID
                      AND a.BankID=(p_BankID)

									GROUP BY
											a.ACCFRom), 0.000) AS LocalBalance,
								IFNULL((SELECT
											SUM(x.Credit) - SUM(x.Debit)
									 FROM
											ExSyAccountsCurrency_AccSafeActivityTb AS x
											left join TB_Users as b on x.AccIDFrom=accid
									WHERE

												 x.OperationTypeID<>34
											AND (x.TypeID = 1 or x.TypeID = 1)
											AND x.CurrencyID = z.ID

											AND AccIDTo<>b.AccID
											AND x.AccBranchID=(p_BranchID)
                      AND x.AccIDTo=(p_BankID)



											), 0.000) AS costmerForPreseEn,

											IFNULL((SELECT
											SUM(x.Credit) - SUM(x.Debit)
									 FROM
											ExSyAccountsCurrency_AccSafeActivityTb AS x
									WHERE

											 x.OperationTypeID=1
											AND x.TypeID = 1
											AND x.CurrencyID = z.ID
											AND x.AccBranchID=(p_BranchID)
                      AND x.AccIDTo=(p_BankID)

											), 0.000) AS SafeTransfer,
								(SELECT c.CurrencyPower
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
              NewCurrencyPriceOwnDetailsTb AS c
                ON b.ID = c.CurrencyIDTo

									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 3
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID
                      AND a.BankID=(p_BankID)


									GROUP BY
											b.CuName,
											c.CurrencyPower) AS CurrencyPower,

								z.ID
						FROM
								CurrencyMainTb AS z
						WHERE
								z.ID <> 1) AS z) AS a
              WHERE
                      a.ID = p_FirstCurrency;
SET v_DEbitValue = p_BPrice1 * v_BuyPrice;
              LEAVE proc;

              INSERT INTO ExSyAccountsCurrency2026.CurrencymovementPruse
                    (
                      `ISID`,
                      `ACCFRom`,
                      `ACCCRint0`,
                      `CRedetTO`,
                      `CRedetDL`,
                      `ACC_DEPET_DL`,
                      `ACC_DEPET_TO`,
                      `UESER_INSERt`,
                      `Inseart_Date`,
                      `ISactive`,
                      `BID`,
                      `TYPEFROM`,
                      Purchaseprice,
                      ACCID,
                      Typeofsalebuyorsell,
                      NetSale,
                      salesPurchaseprice,
                      BankID,
                      CountryID
                    )
              VALUES
                      (
                        p_Code, p_FirstCurrency, p_CurrencyTo, 0.000, 0.00, v_DEbitValue, p_BPrice1, p_UeserInset, NOW(), 1, p_BranchID, p_IsCachorBank, v_BuyPrice, p_SaFACCount, 1, v_valueevenueCalculation, p_Purchaseprice, p_BankID, p_CountryID
                      );

              CALL AccSafeActivityTb_Insert(p_UeserInset, 0, p_BPrice1, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SaFACCount, p_SAFFACCTO, 0, 0, v_SafeIDMovement, p_FirstCurrency, 0, 1, v_SafeIDMovement);
              CALL AccSafeActivityTb_Insert(p_UeserInset, 0, v_DEbitValue, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SaFACCount, p_SAFFACCTO, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
              CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice2, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SAFFACCTO, p_SaFACCount, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
            END IF;
          IF p_SAFTypeform = 0
             THEN
              SELECT CASE                                          WHEN a.CurrencyPower = 1                                            THEN                                            a.Costof                                          ELSE                                          1 / a.Costof                                  END INTO v_BuyPrice FROM


                      (SELECT
					z.CuName,
					z.Costof,

					z.Foreignbalance,
					z.LocalBalance,
					z.costmerForPreseEn,
					z.SafeTransfer,

					IFNULL(z.costmerForPreseEn + z.Foreignbalance+z.SafeTransfer, 0.000)		   AS TotlaPrasceFor_Currency,
					IFNULL(LocalBalance + (Costof * costmerForPreseEn), 0.000) AS TotalLocalBalance,
					z.ID,
          CurrencyPower
			FROM
					(   SELECT
								z.CuName,
								IFNULL((SELECT
											CASE 													WHEN c.CurrencyPower = 0 														THEN 														CASE 																WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																	THEN 																	0.00 																ELSE 																(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) / (SUM(CRedetDL) - SUM(ACC_DEPET_DL)) 														END 													ELSE 													CASE 															WHEN SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO) = 0 																THEN 																0.00 															ELSE 															(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) / (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) 													END 											END AS Costof
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
              NewCurrencyPriceOwnDetailsTb AS c
                ON b.ID = c.CurrencyIDTo
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower), 0.00) AS Costof,

								IFNULL((SELECT
											(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) AS Foreignbalance
									FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID


									GROUP BY
											a.ACCFRom), 0.000) AS Foreignbalance,


								IFNULL((SELECT
											(SUM(CRedetDL) - SUM(ACC_DEPET_DL)) AS LocalBalance
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS a
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID

									GROUP BY
											a.ACCFRom), 0.000) AS LocalBalance,
								IFNULL((SELECT
											SUM(x.Credit) - SUM(x.Debit)
									 FROM
											ExSyAccountsCurrency_AccSafeActivityTb AS x
											left join TB_Users as b on x.AccIDFrom=accid
									WHERE

												 x.OperationTypeID=40

											AND x.CurrencyID = z.ID

											AND AccIDTo<>b.AccID
											AND x.AccBranchID=(p_BranchID)

											), 0.000) AS costmerForPreseEn,

											IFNULL((SELECT
											SUM(x.Credit) - SUM(x.Debit)
									 FROM
											ExSyAccountsCurrency_AccSafeActivityTb AS x
									WHERE

											 x.OperationTypeID=34
											AND x.TypeID <> 1
											AND x.CurrencyID = z.ID
											AND x.AccBranchID=(p_BranchID)

											), 0.000) AS SafeTransfer,
								(SELECT c.CurrencyPower
									 FROM
											ExSyAccountsCurrency_CurrencymovementPruse AS
											a
										INNER JOIN
											CurrencyMainTb AS b
												ON a.ACCFRom = b.ID
										INNER JOIN
              NewCurrencyPriceOwnDetailsTb AS c
                ON b.ID = c.CurrencyIDTo
									WHERE
											a.ISactive = 1
											AND a.TYPEFROM = 0
											AND a.BID=(p_BranchID)
											AND a.ACCFRom = z.ID


									GROUP BY
											b.CuName,
											c.CurrencyPower) AS CurrencyPower,

								z.ID
						FROM
								CurrencyMainTb AS z
						WHERE
								z.ID <> 1) AS z) AS a
              WHERE
                      a.ID = p_FirstCurrency;

              SELECT a.FatherPerintACC, a.EMFtherPRint, a.ACCFPrintCridet INTO v_FatherPerintACC, v_FAtherPERints, v_ACCFPrintCridet FROM
                      CurrencyMainTb AS a
              WHERE
                      a.ID = p_FirstCurrency
 ORDER BY a.ID DESC LIMIT 1;



              SELECT ACCID INTO v_SAFACCAS FROM
                      AccountsTb AS a
              WHERE
                      a.AccParent = v_FatherPerintACC
                      AND a.BranchID = p_BranchID;

              SELECT ACCID INTO v_FirstCurrencysASSS FROM
                      AccountsTb AS a
              WHERE
                      a.AccParent = v_FatherPerintACC
                      AND a.BranchID = p_BranchID;
SET v_DEbitValue = p_BPrice1 * v_BuyPrice;
              INSERT INTO ExSyAccountsCurrency2026.CurrencymovementPruse
                    (
                      `ISID`,
                      `ACCFRom`,
                      `ACCCRint0`,
                      `CRedetTO`,
                      `CRedetDL`,
                      `ACC_DEPET_DL`,
                      `ACC_DEPET_TO`,
                      `UESER_INSERt`,
                      `Inseart_Date`,
                      `ISactive`,
                      `BID`,
                      `TYPEFROM`,
                      Purchaseprice,
                      ACCID,
                      Typeofsalebuyorsell,
                      NetSale,
                      salesPurchaseprice,
                      BankID,
                      CountryID
                    )
              VALUES
                      (
                        p_Code, p_FirstCurrency, p_CurrencyTo, 0.000, 0.00, v_DEbitValue, p_BPrice1, p_UeserInset, NOW(), 1, p_BranchID, 0, v_BuyPrice, v_SAFACCAS, 1, v_valueevenueCalculation, p_Purchaseprice, p_BankID, p_CountryID
                      );

              CALL AccSafeActivityTb_Insert(p_UeserInset, 0, p_BPrice1, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_SAFACCAS, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_FirstCurrency, 0, 1, v_SafeIDMovement);
              CALL AccSafeActivityTb_Insert(p_UeserInset, 0, v_DEbitValue, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_SAFACCAS, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
              CALL AccSafeActivityTb_Insert(p_UeserInset, p_BPrice2, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, p_SAFFACCTO, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
            END IF;








        END IF;
SET v_DEbitValue = p_BPrice1 * v_BuyPrice;SET v_valueevenueCalculation = p_BPrice2 - v_DEbitValue;
      IF v_valueevenueCalculation > 0
         THEN

          IF (p_IsCachorBank = 0
            OR
            p_IsCachorBank = 3)
             THEN
              SELECT FathersRevenueAccount INTO v_FatherPerintACC FROM
                      `CurrencyMainTb` AS a
              WHERE
                      a.ID = p_FirstCurrency
 ORDER BY a.ID DESC LIMIT 1;
              SELECT ACCID INTO v_ACCRevenueCalculation FROM
                      AccountsTb AS a
              WHERE
                      a.AccParent = v_FatherPerintACC
                      AND a.BranchID = p_BranchID;
            END IF;
          IF p_IsCachorBank = 1
             THEN
              SELECT ACCID INTO v_ACCRevenueCalculation FROM
                      AccountsTb AS a
              WHERE
                      a.ACCID = 1482;
            END IF;
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 1, p_BPrice1, p_Purchaseprice);
          CALL AccSafeActivityTb_Insert(p_UeserInset, 0, v_valueevenueCalculation, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_ACCRevenueCalculation, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
        END IF;


      IF v_valueevenueCalculation < 0
         THEN
          IF (p_IsCachorBank = 0
            OR
            p_IsCachorBank = 3)
             THEN
              SELECT FathersLossAccount INTO v_FatherPerintACC FROM
                      `CurrencyMainTb` AS a
              WHERE
                      a.ID = p_FirstCurrency
 ORDER BY a.ID DESC LIMIT 1;
              SELECT ACCID INTO v_ACCRevenueCalculation FROM
                      AccountsTb AS a
              WHERE
                      a.AccParent = v_FatherPerintACC
                      AND a.BranchID = p_BranchID;
            END IF;
          IF p_IsCachorBank = 1
             THEN
              SELECT ACCID INTO v_ACCRevenueCalculation FROM
                      AccountsTb AS a
              WHERE
                      a.ACCID = 1484;
            END IF;
SET v_MovementType   = 'بيع عملة مقابل دينار الليبي';
SET v_SafeIDMovement = GETSafeIDMovement(p_FirstCurrency, p_SAFTypeform, p_IsCachorBank, p_SafTypeTo, p_SAFFACCTO, p_UeserInset, p_SaFACCount, 1, p_BPrice1, p_Purchaseprice);SET v_valueevenueCalculation = v_DEbitValue - p_BPrice2;
          CALL AccSafeActivityTb_Insert(p_UeserInset, v_valueevenueCalculation, 0, v_getDate, p_Notes, p_Code, 1, 1, p_BranchID, v_ACCRevenueCalculation, v_FirstCurrencysASSS, 0, 0, v_SafeIDMovement, p_CurrencyTo, 0, 1, v_SafeIDMovement);
        END IF;




      IF v_IsDefalut = 0
         THEN

          SET p_MSGSTatues = 0;
          SET p_MsgBox = '  عذرا هذه الميزة غير متوفره في الوقت الحالي الرجاء المحاولة في وقت لاحق لانه تحت التطوير الان في الوقت هذه متوفر الشراء فقط بالدينار الليبــــي ';
          ROLLBACK;
          LEAVE proc;
        END IF;




      COMMIT;
      SET p_MSGSTatues = 1;
  END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `NewCurrencyPricesOwnTb_Insert`;
DELIMITER //
CREATE PROCEDURE `NewCurrencyPricesOwnTb_Insert`(IN `p_ID` BIGINT, IN `p_CountryID` INT, IN `p_PriceType` INT, IN `p_ADDueser` INT, IN `p_BranchID` INT, IN `p_AccountType` INT, IN `p_BankID` INT, IN `p_CurrencyPower` TINYINT(1), IN `p_IsUpdate` TINYINT(1), IN `p_ServiceTypeID` INT)
BEGIN
DECLARE v_AgentID INT;
DECLARE v_DefCurrency INT;
DECLARE v_NewID BIGINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;








        SELECT ID INTO v_DefCurrency FROM CurrencyMainTb
        WHERE IsDefault = 1
 ORDER BY CurrencyMainTb.ID DESC LIMIT 1;




        IF EXISTS (
            SELECT 1
            FROM NewCurrencyPricesOwnTb a
            INNER JOIN NewCurrencyPriceOwnDetailsTb b ON a.ID = b.CPID
            INNER JOIN tvp_TypeTb x
                ON b.CurrencyIDFrom = x.CurrencyIDFrom
               AND b.CurrencyIDTo = x.CurrencyIDTo
            WHERE a.CountryID = p_CountryID
              AND a.PriceType = p_PriceType
              AND a.BranchID = p_BranchID
              AND a.BankID=p_BankID
        )
         THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='هذا السعر موجود مسبقا، يرجى استبدال عملية التسعير';END IF;




        SET v_AgentID = CASE WHEN p_AccountType = 1 THEN p_BranchID ELSE 0 END;




        INSERT INTO NewCurrencyPricesOwnTb
        (
            InsertDate,
            InsertTime,
            CountryID,
            PriceType,
            ADDueser,
            BranchID,
            AccountType,
            BankID,
            ServiceTypeID,
            IsActive,
            CurrencyIDFrom
        )
        VALUES
        (
            NOW(),
            DATE_FORMAT(NOW(), '%H:%i:%s'),
            p_CountryID,
            p_PriceType,
            p_ADDueser,
            v_AgentID,
            p_AccountType,
            p_BankID,
            p_ServiceTypeID,
            1,
            v_DefCurrency
        );




        SET v_NewID = LAST_INSERT_ID();




        INSERT INTO NewCurrencyPriceOwnDetailsTb
        (
            CPID,
            CurrencyIDFrom,
            CurrencyIDTo,
            SalePrice,
            BuyPrice,
            CurrencyPower,
            IsActive
        )
        SELECT
            v_NewID,
            CurrencyIDFrom,
            CurrencyIDTo,
            SalePrice,
            BuyPrice,
            CurrencyPower,
            1
        FROM tvp_TypeTb;






END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `PCSettlementTB_Insert`;
DELIMITER //
CREATE PROCEDURE `PCSettlementTB_Insert`(IN `p_Code` LONGTEXT, IN `p_InsertDate` DATE, IN `p_EMPID` BIGINT, IN `p_BranchID` INT, IN `p_SafeID` BIGINT, IN `p_CurrencyID` INT, IN `p_ISID` TEXT, IN `p_PCVal` DECIMAL(12,3), IN `p_SettlementVal` DECIMAL(12,3), IN `p_Notes` LONGTEXT, IN `p_IDCode` BIGINT, IN `p_AccIDSafeID` BIGINT, IN `p_AccIDPetty` BIGINT, IN `p_IsUpdate` TINYINT(1), IN `p_EMPNAME` VARCHAR(200), INOUT `p_MSGSTatues` INT, INOUT `p_MsgBox` LONGTEXT)
proc: BEGIN
DECLARE v_GETID VARCHAR(300);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_MovementType VARCHAR(100);
DECLARE v_SafeIDMovement   VARCHAR(200);
DECLARE v_AccIDPettyBranch BIGINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_MSGSTatues = 0; END;


    START TRANSACTION;
SET v_NetVal = p_PCVal - p_SettlementVal;
    SELECT a.accid INTO v_AccIDPettyBranch FROM
            AccountsTb AS a
    WHERE
            a.AccParent LIKE '%101070%'
            AND a.BranchID = p_BranchID;
    IF p_IsUpdate = 0
       THEN
        SELECT Code INTO v_GETID FROM
                PCSettlementTB AS a
        WHERE
                a.Code = p_Code;
        IF p_Code = v_GETID
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = 'عذرا كود العملية محجوز مسبقا الرجاء الحفظ مرة أخرى';
            ROLLBACK;
            LEAVE proc;
          END IF;
        UPDATE
                PettyCashTb
        SET
                PettyCashTb.IsSettlement = 1
        WHERE
          Code = p_ISID;

        INSERT INTO PCSettlementTB
              (
                Code,
                InsertDate,
                EMPID,
                BranchID,
                SafeID,
                CurrencyID,
                ISID,
                PCVal,
                SettlementVal,
                NetTotal,
                Notes,
                IsActive,
                IDcode,
                AccSafeID,
                AccIDPC
              )
        VALUES
                (
                  p_Code, p_InsertDate, p_EMPID, p_BranchID, p_SafeID, p_CurrencyID, p_ISID, p_PCVal, p_SettlementVal, v_NetVal, p_Notes, 1, p_IDCode, p_AccIDSafeID, v_AccIDPettyBranch
                );
SET v_MovementType = CONCAT(' تسوية عهدة موظف رقم ', CHAR(13), p_ISID);
        INSERT INTO ExSyAccounts_AccSafeActivityTb
              (

                SafeID,
                Debit,
                Credit,
                InsertDate,
                ISID,
                IsActive,
                TypeID,
                OperationTypeID,
                AccBranchID,
                AccIDFrom,
                AccIDTo,
                IsConfirmed,
                IsCanceled,
                MovementType,
                CurrencyID,
                DailyClosed,
                SafeIDDailyClose,
                Note
              )
        VALUES
                (

                  p_SafeID,
                  0.000,
                  p_PCVal,
                  p_InsertDate,
                  p_Code,
                  1,
                  14,
                  44,
                  p_BranchID,
                  v_AccIDPettyBranch,
                  p_AccIDSafeID,
                  1,
                  0,
                  v_MovementType,
                  p_CurrencyID,
                  0,
                  0,
                  p_Notes
                );
        IF p_SettlementVal < p_PCVal
           THEN
SET v_SafeIDMovement = CONCAT('تسوية عهدة الموظف', ' ', p_EMPNAME, ' ', CHAR(13), p_ISID);
            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note,
                    SafeIDMovement
                  )
            VALUES
                    (

                      p_SafeID,
                      v_NetVal,
                      0.000,
                      p_InsertDate,
                      p_Code,
                      1,
                      14,
                      44,
                      p_BranchID,
                      p_AccIDSafeID,
                      v_AccIDPettyBranch,
                      1,
                      0,
                      v_MovementType,
                      p_CurrencyID,
                      0,
                      0,
                      p_Notes, v_SafeIDMovement
                    );
          END IF;
        IF p_SettlementVal > p_PCVal
           THEN
SET v_NetVal = p_SettlementVal - p_PCVal;
SET v_SafeIDMovement = CONCAT('تسوية عهدة الموظف', ' ', p_EMPNAME, ' ', CHAR(13), p_ISID);
            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note,
                    SafeIDMovement
                  )
            VALUES
                    (

                      p_SafeID,
                      0.000,
                      v_NetVal,
                      p_InsertDate,
                      p_Code,
                      1,
                      14,
                      44,
                      p_BranchID,
                      p_AccIDSafeID,
                      v_AccIDPettyBranch,
                      1,
                      0,
                      v_MovementType,
                      p_CurrencyID,
                      0,
                      0,
                      p_Notes, v_SafeIDMovement
                    );
          END IF;



        INSERT INTO PCSettlementDetailsTB
              (
                PCISID,
                SPCISID,
                ExpensVal,
                AccIDEX,
                EXID,
                Notes,
                IsActive
              )
            SELECT
                    p_ISID,
                    p_Code,
                    a.ExpensVal,
                    a.AccIDEX,
                    a.EXID,
                    a.NotesDe,
                    1
            FROM
                    tvp_type AS a;


        BEGIN
SET v_MovementType = CONCAT(' تسوية عهدة موظف رقم ', CHAR(13), p_ISID);
          IF p_SettlementVal < p_PCVal
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (

                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      IsConfirmed,
                      IsCanceled,
                      MovementType,
                      CurrencyID,
                      DailyClosed,
                      SafeIDDailyClose,
                      Note
                    )
                  SELECT


                          p_SafeID,
                          a.ExpensVal,
                          0.000,
                          p_InsertDate,
                          p_Code,
                          1,
                          14,
                          44,
                          p_BranchID,
                          a.AccIDEX,
                          p_AccIDSafeID,
                          1,
                          0,
                          v_MovementType,
                          p_CurrencyID,
                          0,
                          0,
                          a.NotesDe
                  FROM
                          tvp_type AS a;
            END IF;
          IF p_SettlementVal = p_PCVal
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (

                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      IsConfirmed,
                      IsCanceled,
                      MovementType,
                      CurrencyID,
                      DailyClosed,
                      SafeIDDailyClose,
                      Note
                    )
                  SELECT


                          p_SafeID,
                          a.ExpensVal,
                          0.000,
                          p_InsertDate,
                          p_Code,
                          1,
                          14,
                          44,
                          p_BranchID,
                          a.AccIDEX,
                          p_AccIDSafeID,
                          1,
                          0,
                          v_MovementType,
                          p_CurrencyID,
                          0,
                          0,
                          a.NotesDe
                  FROM
                          tvp_type AS a;
            END IF;
          IF p_SettlementVal > p_PCVal
             THEN

              INSERT INTO ExSyAccounts_AccSafeActivityTb
                    (

                      SafeID,
                      Debit,
                      Credit,
                      InsertDate,
                      ISID,
                      IsActive,
                      TypeID,
                      OperationTypeID,
                      AccBranchID,
                      AccIDFrom,
                      AccIDTo,
                      IsConfirmed,
                      IsCanceled,
                      MovementType,
                      CurrencyID,
                      DailyClosed,
                      SafeIDDailyClose,
                      Note
                    )
                  SELECT


                          p_SafeID,
                          a.ExpensVal,
                          0.000,
                          p_InsertDate,
                          p_Code,
                          1,
                          14,
                          44,
                          p_BranchID,
                          a.AccIDEX,
                          p_AccIDSafeID,
                          1,
                          0,
                          v_MovementType,
                          p_CurrencyID,
                          0,
                          0,
                          a.NotesDe
                  FROM
                          tvp_type AS a;
            END IF;
        END;
      END IF;
    IF p_IsUpdate = 1
       THEN
        UPDATE
                PCSettlementTB
        SET
                PCSettlementTB.IsActive = 0
        WHERE
          PCSettlementTB.Code = p_Code;



        INSERT INTO ExSyAccounts_AccSafeActivityTb
              (

                SafeID,
                Debit,
                Credit,
                InsertDate,
                ISID,
                IsActive,
                TypeID,
                OperationTypeID,
                AccBranchID,
                AccIDFrom,
                AccIDTo,
                IsConfirmed,
                IsCanceled,
                MovementType,
                CurrencyID,
                DailyClosed,
                SafeIDDailyClose,
                Note,
                SafeIDMovement
              )
        VALUES
                (

                  p_SafeID,
                  p_PCVal,
                  0.000,
                  p_InsertDate,
                  p_Code,
                  1,
                  14,
                  44,
                  p_BranchID,
                  p_AccIDSafeID,
                  v_AccIDPettyBranch,
                  1,
                  0,
                  v_MovementType,
                  p_CurrencyID,
                  0,
                  0,
                  p_Notes, CONCAT('معالجة خطأ في عهدة الموظف', ' ', p_EMPNAME)
                );
SET v_MovementType = CONCAT(' معالجة خطأ في تسوية عهدة رقم ', CHAR(13), p_ISID);
        INSERT INTO ExSyAccounts_AccSafeActivityTb
              (

                SafeID,
                Debit,
                Credit,
                InsertDate,
                ISID,
                IsActive,
                TypeID,
                OperationTypeID,
                AccBranchID,
                AccIDFrom,
                AccIDTo,
                IsConfirmed,
                IsCanceled,
                MovementType,
                CurrencyID,
                DailyClosed,
                SafeIDDailyClose,
                Note
              )
        VALUES
                (

                  p_SafeID,
                  p_PCVal,
                  0.000,
                  p_InsertDate,
                  p_Code,
                  1,
                  14,
                  44,
                  p_BranchID,
                  v_AccIDPettyBranch,
                  p_AccIDSafeID,
                  1,
                  0,
                  v_MovementType,
                  p_CurrencyID,
                  0,
                  0,
                  p_Notes
                );
        IF p_SettlementVal < p_PCVal
           THEN
SET v_SafeIDMovement = CONCAT('معالجة خطأ عهدة الموظف', CHAR(13), p_EMPNAME, CHAR(13), p_ISID);
            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note,
                    SafeIDMovement
                  )
            VALUES
                    (

                      p_SafeID,
                      0.000,
                      v_NetVal,
                      p_InsertDate,
                      p_Code,
                      1,
                      14,
                      44,
                      p_BranchID,
                      p_AccIDSafeID,
                      v_AccIDPettyBranch,
                      1,
                      0,
                      v_MovementType,
                      p_CurrencyID,
                      0,
                      0,
                      p_Notes, v_SafeIDMovement
                    );
          END IF;
        IF p_SettlementVal > p_PCVal
           THEN
SET v_NetVal = p_SettlementVal - p_PCVal;
SET v_SafeIDMovement = CONCAT('معالجة خطأ عهدة الموظف', CHAR(13), p_EMPNAME, CHAR(13), p_ISID);
            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note,
                    SafeIDMovement
                  )
            VALUES
                    (

                      p_SafeID,
                      v_NetVal,
                      0.000,
                      p_InsertDate,
                      p_Code,
                      1,
                      14,
                      44,
                      p_BranchID,
                      p_AccIDSafeID,
                      v_AccIDPettyBranch,
                      1,
                      0,
                      v_MovementType,
                      p_CurrencyID,
                      0,
                      0,
                      p_Notes, v_SafeIDMovement
                    );
          END IF;
        UPDATE
                PCSettlementDetailsTB
        SET
                PCSettlementDetailsTB.IsActive = 0
        WHERE
          PCSettlementDetailsTB.SPCISID = p_Code;
        IF p_SettlementVal < p_PCVal
           THEN

            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note
                  )
                SELECT


                        p_SafeID,
                        0.000,
                        A.ExpensVal,
                        p_InsertDate,
                        p_Code,
                        1,
                        14,
                        44,
                        p_BranchID,
                        A.AccIDEX,
                        p_AccIDSafeID,
                        1,
                        0,
                        v_MovementType,
                        p_CurrencyID,
                        0,
                        0,
                        A.NotesDe
                FROM
                        tvp_type AS A;
          END IF;
        IF p_SettlementVal = p_PCVal
           THEN

            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note
                  )
                SELECT


                        p_SafeID,
                        0.000,
                        A.ExpensVal,
                        p_InsertDate,
                        p_Code,
                        1,
                        14,
                        44,
                        p_BranchID,
                        A.AccIDEX,
                        p_AccIDSafeID,
                        1,
                        0,
                        v_MovementType,
                        p_CurrencyID,
                        0,
                        0,
                        A.NotesDe
                FROM
                        tvp_type AS A;
          END IF;
        IF p_SettlementVal > p_PCVal
           THEN

            INSERT INTO ExSyAccounts_AccSafeActivityTb
                  (

                    SafeID,
                    Debit,
                    Credit,
                    InsertDate,
                    ISID,
                    IsActive,
                    TypeID,
                    OperationTypeID,
                    AccBranchID,
                    AccIDFrom,
                    AccIDTo,
                    IsConfirmed,
                    IsCanceled,
                    MovementType,
                    CurrencyID,
                    DailyClosed,
                    SafeIDDailyClose,
                    Note
                  )
                SELECT

                        p_SafeID,
                        0.000,
                        A.ExpensVal,
                        p_InsertDate,
                        p_Code,
                        1,
                        14,
                        44,
                        p_BranchID,
                        A.AccIDEX,
                        p_AccIDSafeID,
                        1,
                        0,
                        v_MovementType,
                        p_CurrencyID,
                        0,
                        0,
                        A.NotesDe
                FROM
                        tvp_type AS A;
          END IF;
      END IF;
    COMMIT;
    SET p_MSGSTatues = 1;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `TaxiInvoiceDrivers_insert`;
DELIMITER //
CREATE PROCEDURE `TaxiInvoiceDrivers_insert`(IN `p_Code` LONGTEXT, IN `p_BranchID` INT, IN `p_Driver_ID` INT, IN `p_DriversShare` DECIMAL(18,3), IN `p_ueserIinsert` INT, IN `p_Notes` LONGTEXT, IN `p_ID_Code` INT, INOUT `p_ismasg` INT, INOUT `p_masge` LONGTEXT, IN `p_safeID_ACID` INT)
proc: BEGIN
DECLARE v_ID int;
DECLARE v_ID_code_Trexet int;
DECLARE v_wallT_FromSAfe FLOAT;
DECLARE v_Total_Values float;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; set p_ismasg =  2; CALL ERROR_PROC(); END;
START TRANSACTION;

SELECT ID_Code INTO v_ID_code_Trexet FROM TaxiInvoiceDrivers as a where ID_Code = p_ID_Code;
if v_ID_code_Trexet = p_ID_Code
 THEN
set p_ismasg = 0;
set p_masge = 'عذرا هذه الكود موجود مسبا الرجاء المحاولة في وقت لاحق';
rollback;
LEAVE proc;
END IF;


INSERT INTO `TaxiInvoiceDrivers`
           (`Code`,`BranchID`,`Driver_ID`,`insertDate`,`date_time`
,`Total_ValueTransfers`,`TotalTaxi`,`DriversShare`,`IsACtiver`
,`Delivery_Status`,`ueserIinsert`,`Notes`,`ID_Code`,safeID_ACID)
values( p_Code , p_BranchID  , p_Driver_ID ,NOW() , NOW() , 0 , 0 ,p_DriversShare ,
1,0,p_ueserIinsert , p_Notes , p_ID_Code,p_safeID_ACID);
		SELECT ID INTO v_ID FROM TaxiInvoiceDrivers as a where a.code  = p_Code;

	UPDATE InternalEx as a inner join tvp_InternalEx_type_isertAStaxi as b on a.ID= b.ID SET ID_Delvre_For_Taxie = v_ID , Driver_ID = 	p_Driver_ID , ConfirmType = 9;

 SELECT IFNULL(Sum(a.Debit) , 0 ) - IFNULL(sum(a.Credit) ,0) INTO v_wallT_FromSAfe FROM  `EX24AccSafeActivityTb` as a
inner join TaxiInvoiceDrivers as b on a.AccIDFrom = b.safeID_ACID where b.safeID_ACID = p_safeID_ACID;




SELECT IFNULL(a.Total_ValueTransfers,0) -IFNULL(a.TotalTaxi,0) INTO v_Total_Values FROM TaxiInvoiceDrivers as a
inner join InternalEx as b  on a.ID =b.ID_Delvre_For_Taxie
where a.Code = p_Code
group by b.ID ,a.Total_ValueTransfers , a.TotalTaxi
 ORDER BY b.ID DESC, a.Total_ValueTransfers DESC, a.TotalTaxi DESC LIMIT 1;



if v_Total_Values> v_wallT_FromSAfe
 THEN
set p_ismasg = 0;
set p_masge =  'قيمة الحوالات اكبر من الرصيد الموجود في الخزينة الرجاء التاكد من القيمة ';
rollback;
LEAVE proc;
END IF;



INSERT INTO `EX24AccSafeActivityTb`
           (`SafeID`
           ,`Debit`
           ,`Credit`
           ,`InsertDate`
           ,`Description`
           ,`ISID`
           ,`IsActive`
           ,`TypeID`
           ,`OperationTypeID`
           ,`AccBranchID`
           ,`AccIDFrom`
           ,`AccIDTo`
           ,`IsConfirmed`
           ,`IsCanceled`
           ,`MovementType`
           ,`CurrencyID`
           ,`DailyClosed`
           ,`SafeIDDailyClose`
           ,`Note`
           ,`SafeIDMovement`
		  , inesrtMobile
         )

		 select 54   ,  b.OverallVal - b.TaxiValues ,0   ,NOW() , 'فاتورة نقل داخلي' ,b.Code ,1 ,67,67 , a.BranchID ,c.accontID,A.safeID_ACID ,0,0
		 ,'فاتورة ارسال مع تاكسي'  ,b.RecievedCurrencyID ,  92 , 92,'لايوجد ملاحظات' , '' ,b.Type_Moble from TaxiInvoiceDrivers as a
		 inner join InternalEx as b on a.ID=b.ID_Delvre_For_Taxie
		 inner join DriversTb as c on a.Driver_ID = c.ID
		 where a.code = p_Code;

INSERT INTO `EX24AccSafeActivityTb`
           (`SafeID`
           ,`Debit`
           ,`Credit`
           ,`InsertDate`
           ,`Description`
           ,`ISID`
           ,`IsActive`
           ,`TypeID`
           ,`OperationTypeID`
           ,`AccBranchID`
           ,`AccIDFrom`
           ,`AccIDTo`
           ,`IsConfirmed`
           ,`IsCanceled`
           ,`MovementType`
           ,`CurrencyID`
           ,`DailyClosed`
           ,`SafeIDDailyClose`
           ,`Note`
           ,`SafeIDMovement`
		  , inesrtMobile
         )

		 select 54  ,0  ,  b.OverallVal - b.TaxiValues    ,NOW() , 'فاتورة نقل داخلي' ,b.Code ,1 ,67,67 , a.BranchID ,A.safeID_ACID,c.accontID ,0,0
		 ,'فاتورة ارسال مع تاكسي'  ,b.RecievedCurrencyID ,  92 , 92,'لايوجد ملاحظات' , '' ,b.Type_Moble from TaxiInvoiceDrivers as a
		 inner join InternalEx as b on a.ID=b.ID_Delvre_For_Taxie
		 inner join DriversTb as c on a.Driver_ID = c.ID
		  where a.code = p_Code;

UPDATE `Request_to_summon_driversTB` as a
inner join DriversTb as b on a.AccID_Accipe= b.accontID
inner join TaxiInvoiceDrivers as c on b.ID = c.Driver_ID SET IsAccpit = 2 where a.IsAccpit = 1;
		commit;
		set p_ismasg =  1;
		   END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `UserAccessProfileTemplate_ID_inser_for_update`;
DELIMITER //
CREATE PROCEDURE `UserAccessProfileTemplate_ID_inser_for_update`(IN `p_UserID` INT, IN `p_profid` INT)
BEGIN
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); END;
      START TRANSACTION;

      DELETE FROM
      `Ueser_Group_main_ID`
      WHERE
              UserID = p_UserID;
      DELETE FROM
      `UserROOLMAINPROFILETP_ID`
      WHERE
              Userid = p_UserID;
      DELETE FROM
      FrmScreensTb_Details_UESIRID
      WHERE
              UeserID = p_UserID;
      DELETE FROM
      NotificationUSERID
      WHERE
              userID = p_UserID;


      INSERT INTO `UserROOLMAINPROFILETP_ID`
            (
              `profid`,
              `canshow`,
              `mainid`,
              `Userid`
            )
          SELECT
                  p_profid,
                  a.canshow,
                  a.mainid,
                  p_UserID
          FROM
                  tvp_UserROOLMAINPROFILETP_ID_insert_forupdate AS a;



      INSERT INTO `Ueser_Group_main_ID`
            (
              `Profile`,
              `GrouID`,
              `Canshow`,
              `ManID`,
              `UserID`
            )
          SELECT
                  p_profid,
                  a.GrouID,
                  a.Canshow,
                  a.ManID,
                  p_UserID
          FROM
                  tvp_Ueser_Group_main_ID_ype AS a;


      INSERT INTO `FrmScreensTb_Details_UESIRID`
            (
              `ScreenID`,
              `Can_branch`,
              `Can_safID`,
              `Can_Close_safid`,
              `Can_Accfrom`,
              `Can_accTo`,
              `Can_ISbacnk`,
              `UeserID`
            )
          SELECT
                  a.ScreenID,
                  `Can_branch`,
                  `Can_safID`,
                  `Can_Close_safid`,
                  `Can_Accfrom`,
                  `Can_accTo`,
                  Can_ISbacnk,
                  p_UserID
          FROM
                  FrmScreensTb_Details AS a
          GROUP BY
                  a.ScreenID,
                  `Can_branch`,
                  `Can_safID`,
                  `Can_Close_safid`,
                  `Can_Accfrom`,
                  `Can_accTo`,
                  Can_ISbacnk;



      UPDATE FrmScreensTb_Details_UESIRID AS a
          INNER JOIN
            tvp_FrmScreensTb_Details_UESIRID AS b
              ON a.ScreenID = b.ScreenID
                AND a.UeserID = p_UserID SET `Can_branch` = b.Can_branch,
              `Can_safID` = b.Can_safID,
              `Can_Close_safid` = b.Can_Close_safid,
              `Can_Accfrom` = b.Can_Accfrom,
              `Can_accTo` = b.Can_accTo,
              Can_ISbacnk = b.Can_accTo;
      UPDATE `FrmScreensTb_Details_UESIRID` AS a
          INNER JOIN
            tvp_FrmScreensTb_Details_ForUpdate AS b
              ON a.`ScreenID` = b.`ScreenID`
                AND a.`UeserID` = b.`UeserID` SET CaN_calCylaTion = b.CaN_calCylaTion,
              Can_Close_safid = b.`Can_Close_safid ` WHERE
        a.`UeserID` = p_UserID;
      UPDATE `FrmScreensTb_Details_UESIRID` AS a
          INNER JOIN
            tvp_FrmScreensTb_Details_UESIRID_can_banck AS b
              ON a.`ScreenID` = b.`ScreenID`
                AND a.`UeserID` = b.`UeserID` SET can_banck = b.can_banck,
              can_cash = b.can_cash,
              can_Acount = b.can_Acount WHERE
        a.`UeserID` = p_UserID;
      INSERT INTO `NotificationUSERID`
            (
              `profileID`,
              `NotificationID`,
              `canshow`,
              `userID`
            )
          SELECT
                  p_profid,
                  a.NotificationID,
                  canshow,
                  p_UserID
          FROM
                  tvp_NotificationUSERID_Type AS a;
      COMMIT;


  END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `UserAccessProfileTemplate_ID_update`;
DELIMITER //
CREATE PROCEDURE `UserAccessProfileTemplate_ID_update`(IN `p_UeserID` INT, INOUT `p_msg` LONGTEXT, INOUT `p_MSGStat` INT)
BEGIN
DECLARE v_IDPROFILE INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; SET p_MSGStat = 0; CALL ERROR_PROC(); END;
        START TRANSACTION;

	SELECT `UserType` INTO v_IDPROFILE FROM `TB_Users` where `USID`=p_UeserID
 ORDER BY TB_Users.USID DESC LIMIT 1;
        DELETE FROM `UserAccessProfileTemplate_ID`
        WHERE UserID = p_UeserID;


        INSERT INTO `UserAccessProfileTemplate_ID`
           (`ProfileID`
           ,`MainID`
           ,`Group_ID`
           ,`ScreenID`
           ,`CanShow`
           ,`CanSave`
           ,`CanEdit`
           ,`CanDelete`
           ,`CanSearch`
           ,`CanPrint`
           ,`UserID`)
        SELECT
            v_IDPROFILE,
            `MainID`,
            `Group_ID`,
            `ScreenID`,
            `CanShow`,
            `CanSave`,
            `CanEdit`,
            `CanDelete`,
            `CanSearch`,
            `CanPrint`,
            p_UeserID
        FROM tvp_UserAccessProfileTemplate_ID_insertype AS A;



        COMMIT;


        SET p_MSGStat = 1;


END //
DELIMITER ;

