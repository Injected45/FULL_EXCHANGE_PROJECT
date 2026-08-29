-- Production push: UI-test fixes (2026-07-27)
-- (1) T-SQL non-NULL param defaults re-applied (IsActive/OPType/etc. class)
-- (2) ACCOUNTSTB_selectmax: leading BEGIN...END grouping block was dropped -> no auto account number
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `Companies_Crud`;
DELIMITER //
CREATE PROCEDURE `Companies_Crud`(IN `p_Action` INT, IN `p_ID` INT, IN `p_CompanyName` TEXT, IN `p_TypeID` INT, IN `p_ParentID` INT, IN `p_EmpID` INT, IN `p_ManagerName` VARCHAR(250), IN `p_LicenseNumber` VARCHAR(150), IN `p_TaxNumber` VARCHAR(50), IN `p_IsActive` TINYINT(1))
proc: BEGIN
DECLARE v_NewID INT DEFAULT LAST_INSERT_ID();
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;
SET p_IsActive = IFNULL(p_IsActive, 1);
BEGIN







        IF p_Action = 0
         THEN
            SELECT
                  C.ID
                  ,c.CompanyName
                , CONCAT(CASE WHEN C.TypeID=0 THEN 'شركة ' ELSE 'قسم ' END, C.CompanyName) AS CompanyName1
                , C.TypeID
                , C.ParentID
                , P.CompanyName AS ParentCompany
                , C.EmpID
                , C.ManagerName
                , C.LicenseNumber
                , C.TaxNumber
                , C.IsActive
            FROM Companies C
            LEFT JOIN Companies P ON P.ID = C.ParentID
            WHERE C.IsActive = 1
              AND (C.TypeID = p_TypeID OR p_TypeID IS NULL)
              AND (C.ParentID = p_ParentID OR p_ParentID IS NULL)
            ORDER BY C.CompanyName
            ;
            LEAVE proc;
        END IF;




        IF p_Action = 1
         THEN
            START TRANSACTION;

            IF EXISTS (SELECT 1 FROM Companies WHERE CompanyName = p_CompanyName AND IsActive = 1)
             THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='اسم الشركة موجود مسبقاً';
                ROLLBACK;
                LEAVE proc;
            END IF;

            IF EXISTS (SELECT 1 FROM Companies ) AND p_ParentID IS NULL
             THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='عذرا حقل الشركة الأم لا يمكن أن يكون فارغا';
                ROLLBACK;
                LEAVE proc;
            END IF;

            IF NOT EXISTS (SELECT 1 FROM Companies ) AND p_ParentID IS NULL
             THEN
 SET p_ParentID=0;
            END IF;

            INSERT INTO Companies
            (
                  CompanyName, TypeID, ParentID, EmpID, ManagerName, LicenseNumber, TaxNumber, IsActive
            )
            VALUES
            (
                  p_CompanyName, p_TypeID, p_ParentID, p_EmpID, p_ManagerName, p_LicenseNumber, p_TaxNumber, p_IsActive
            );



            IF p_EmpID IS NOT NULL AND EXISTS (SELECT 1 FROM EmployeeTb WHERE ID = p_EmpID)
             THEN
                UPDATE EmployeeTb SET Jobgrade = v_NewID WHERE ID = p_EmpID;
            END IF;

            COMMIT;
            SELECT v_NewID AS NewID;
            LEAVE proc;
        END IF;




        IF p_Action = 2
         THEN
            START TRANSACTION;


            IF p_ParentID IS NOT NULL AND p_ParentID = p_ID
             THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='لا يمكن أن تكون الشركة تابعة لنفسها!';
                ROLLBACK;
                LEAVE proc;
            END IF;

            IF EXISTS (SELECT 1 FROM Companies WHERE CompanyName = p_CompanyName AND ID <> p_ID AND IsActive = 1)
             THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='اسم الشركة موجود مسبقاً';
                ROLLBACK;
                LEAVE proc;
            END IF;

            UPDATE Companies
               SET CompanyName   = p_CompanyName, TypeID        = p_TypeID, ParentID      = p_ParentID, EmpID         = p_EmpID, ManagerName   = p_ManagerName, LicenseNumber = p_LicenseNumber, TaxNumber     = p_TaxNumber, IsActive      = p_IsActive
             WHERE ID = p_ID;

 UPDATE EmployeeTb SET Jobgrade = p_ID WHERE ID = p_EmpID;

            COMMIT;
            SELECT p_ID AS UpdatedID;
            LEAVE proc;
        END IF;




        IF p_Action = 3
         THEN
            START TRANSACTION;


            IF EXISTS (SELECT 1 FROM Companies WHERE ParentID = p_ID AND IsActive = 1)
             THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='لا يمكن حذف هذه الشركة لوجود شركات تابعة لها. الرجاء نقل أو حذف الشركات التابعة أولاً.';
                ROLLBACK;
                LEAVE proc;
            END IF;

            UPDATE Companies SET IsActive = 0 WHERE ID = p_ID;

            COMMIT;
            SELECT p_ID AS DeletedID;
            LEAVE proc;
        END IF;




        IF p_Action = 4
         THEN
            SELECT
                  C.ID, C.CompanyName, C.TypeID, C.ParentID, P.CompanyName AS ParentCompany,
                  C.EmpID, C.ManagerName, C.LicenseNumber, C.TaxNumber, C.IsActive
            FROM Companies C
            LEFT JOIN Companies P ON P.ID = C.ParentID
            WHERE C.ID = p_ID;
            LEAVE proc;
        END IF;



END;
END
//
DELIMITER ;

DROP PROCEDURE IF EXISTS `EMPLOYEETB_Insert`;
DELIMITER //
CREATE PROCEDURE `EMPLOYEETB_Insert`(IN `p_CODE` LONGTEXT, IN `p_EMPNAME` LONGTEXT, IN `p_NATNUMBER` LONGTEXT, IN `p_CLASSIFICATION` INT, IN `p_PHONE1` VARCHAR(50), IN `p_PHONE2` VARCHAR(50), IN `p_BIRTHDATE` DATE, IN `p_EMPDATE` DATE, IN `p_PassportNo` LONGTEXT, IN `p_Nationality` LONGTEXT, IN `p_CertificateType` LONGTEXT, IN `p_SalaryVal` DOUBLE, IN `p_EMail` LONGTEXT, IN `p_CanDebit` TINYINT UNSIGNED, IN `p_Jobgrade` INT, IN `p_ISACTIVE` TINYINT(1), IN `p_BranchID` INT, IN `p_IsUpdate` TINYINT(1), IN `p_ID` INT, INOUT `p_MSGSTatues` INT, INOUT `p_MsgBox` LONGTEXT, IN `p_BankID` INT, IN `p_BBranchID` INT, IN `p_AccOwner` LONGTEXT, IN `p_BankNo` LONGTEXT, IN `p_BankSalaryCalc` TINYINT(1), IN `p_ContractID` BIGINT)
proc: BEGIN
DECLARE v_IsExisit INT;
DECLARE v_OldeCustName LONGTEXT;
DECLARE v_CheckName LONGTEXT;
DECLARE v_OldBranchID INT;
DECLARE v_fatherparent DECIMAL(18, 0);
DECLARE v_ADPMNTAccID DECIMAL(18, 0);
DECLARE v_PettyAccID DECIMAL(18, 0);
DECLARE v_acccode DECIMAL(18, 0);
DECLARE v_IDcode BIGINT;
DECLARE v_AccName LONGTEXT;
DECLARE v_BName LONGTEXT;
DECLARE v_AccID BIGINT;
DECLARE v_AccIDPTTCA BIGINT;
DECLARE v_AccIDEx BIGINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_MSGSTatues = 0; END;
SET p_ContractID = IFNULL(p_ContractID, 0);



    START TRANSACTION;





























    IF p_IsUpdate = 0
       THEN
                  SET p_MSGSTatues = 0;
            SET p_MsgBox = 'لا يمكن إضافة موظف  من المنظومة الرجاء الذهاب للموقع';
            ROLLBACK;
            LEAVE proc;
































































































      END IF;

    IF p_IsUpdate = 1
       THEN

        SELECT AccID INTO v_AccID FROM
                EmployeeTb AS ET
        WHERE
                ID = p_ID;
        SELECT ET.AccIDPTTCA INTO v_AccIDPTTCA FROM
                EmployeeTb AS ET
        WHERE
                ID = p_ID;
        SELECT ET.AccIDEx INTO v_AccIDEx FROM
                EmployeeTb AS ET
        WHERE
                ID = p_ID;


        SELECT CT.EMPNAME INTO v_OldeCustName FROM
                EmployeeTb AS CT
        WHERE
                ID = p_ID;

        SELECT CT.BranchID INTO v_OldBranchID FROM
                EmployeeTb AS CT
        WHERE
                ID = p_ID;

        SELECT CT.EMPNAME INTO v_CheckName FROM
                EmployeeTb AS CT
        WHERE
                ID <> p_ID;


        IF v_CheckName = p_EMPNAME
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = 'عذرا هذا الاسم موجود مسبقاً';
            ROLLBACK;
            LEAVE proc;
          END IF;


        IF v_OldeCustName <> p_EMPNAME
          AND
          v_OldBranchID <> p_BranchID
           THEN
            SELECT CB.BName INTO v_BName FROM
                    CoBranch AS CB
            WHERE
                    CB.ID = p_BranchID;
SET v_IDcode = IFNULL((SELECT MAX(xx.IDcode) FROM AccountsTb AS xx WHERE xx.AccParent = v_fatherparent) , 0) + 1;
            UPDATE
                    AccountsTb
            SET
                    AccName = p_EMPNAME, AccCode = CONCAT(CAST(v_fatherparent AS CHAR), '0', v_IDcode), IDcode = v_IDcode, BranchID = p_BranchID
            WHERE
              AccID = v_AccID;
          END IF;

        IF v_OldeCustName = p_EMPNAME
          AND
          v_OldBranchID <> p_BranchID
           THEN
            SELECT CB.BName INTO v_BName FROM
                    CoBranch AS CB
            WHERE
                    CB.ID = p_BranchID;
SET v_acccode = CONCAT(CAST(v_fatherparent AS CHAR), '0', CAST(IFNULL((SELECT MAX(xx.IDcode) FROM AccountsTb AS xx WHERE xx.AccParent = v_fatherparent) , 0) + 1 AS CHAR));
SET v_IDcode = IFNULL((SELECT MAX(xx.IDcode) FROM AccountsTb AS xx WHERE xx.AccParent = v_fatherparent) , 0) + 1;
            UPDATE
                    AccountsTb
            SET
                    AccCode = CONCAT(CAST(v_fatherparent AS CHAR), '0', v_IDcode), IDcode = v_IDcode, BranchID = p_BranchID
            WHERE
              AccID = v_AccID;

          END IF;

        IF v_OldeCustName <> p_EMPNAME
          AND
          v_OldBranchID = p_BranchID
           THEN

            UPDATE
                    AccountsTb
            SET
                    AccName = p_EMPNAME
            WHERE
              AccID = v_AccID;
          END IF;


        UPDATE
                EmployeeTb
        SET
                CODE = p_CODE, EMPNAME = p_EMPNAME, NATNUMBER = p_NATNUMBER,
                `CLASSIFICATION` = p_CLASSIFICATION,
                PHONE1 = p_PHONE1,
                PHONE2 = p_PHONE2,
                BIRTHDATE = p_BIRTHDATE,
                EMPDATE = p_EMPDATE,
                PassportNo = p_PassportNo,
                Nationality = p_Nationality,
                CertificateType = p_CertificateType,
                SalaryVal = p_SalaryVal,
                EMail = p_EMail,
                CanDebit = p_CanDebit,
                Jobgrade = p_Jobgrade,
                ISACTIVE = p_ISACTIVE,
                BranchID = p_BranchID,
                BankID = p_BankID,
                BBranchID = p_BBranchID,
                AccOwner = p_AccOwner,
                BankNo = p_BankNo,
                BankSalaryCalc = p_BankSalaryCalc

        WHERE
          ID = p_ID;

        UPDATE
                AccountsTb
        SET
                AccPhone = p_PHONE1, AccMobile = p_PHONE2, AccEmail = p_EMail, AccIDNo = p_PassportNo, EditDate = NOW(), CanDebit = p_CanDebit, AccNatNumber=p_NATNUMBER
        WHERE
          AccID = v_AccID;
      END IF;

    UPDATE
            TB_Users
    SET
            IsActive = p_ISACTIVE
    WHERE
      EMPID = p_ID;

    COMMIT;
    SET p_MSGSTatues = 1;
END
//
DELIMITER ;

DROP PROCEDURE IF EXISTS `EMPORCUSTWITHDRAWALTB_Insert`;
DELIMITER //
CREATE PROCEDURE `EMPORCUSTWITHDRAWALTB_Insert`(IN `p_Code` TEXT, IN `p_AccParent` BIGINT, IN `p_WDVAL` DECIMAL(12,3), IN `p_SafeID` INT, IN `p_TypeID` TINYINT UNSIGNED, IN `p_CODEID` BIGINT, IN `p_BranchID` INT, IN `p_DPSVAL` DECIMAL(12,3), IN `p_Notes` LONGTEXT, IN `p_IsUpdate` TINYINT(1), IN `p_AccIDFrom` BIGINT, IN `p_CurrencyFrom` INT, INOUT `p_MSG` INT, INOUT `p_MSGBOX` LONGTEXT, IN `p_PaidFor` LONGTEXT, IN `p_Phone` LONGTEXT, IN `p_IDNo` VARCHAR(50), IN `p_OPType` INT)
proc: BEGIN
DECLARE v_AccVal DECIMAL(12, 3);
DECLARE v_CanDEpit BIT;
DECLARE v_LimtedVal DECIMAL(12, 3);
DECLARE v_IsLimted BIT;
DECLARE v_IsDefault BIT;
DECLARE v_CurrDefault INT;
DECLARE v_UserID INT;
DECLARE v_AccName LONGTEXT;
DECLARE v_getDate DATE DEFAULT NOW();
DECLARE v_MovementType LONGTEXT;
DECLARE v_MovementType2 LONGTEXT;
DECLARE v_FAtherPERints BIGINT;
DECLARE v_FirstCurrencyd BIGINT;
DECLARE v_ACCFPrintCridet BIGINT;
DECLARE v_ACCFPrintCridetACC BIGINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_MSGBOX = 0; END;
SET p_OPType = IFNULL(p_OPType, 38);
BEGIN


      START TRANSACTION;




          SELECT cmt.IsDefault INTO v_IsDefault FROM
                  CurrencyMainTb cmt
          WHERE
                  cmt.ID = p_CurrencyFrom
 ORDER BY cmt.ID DESC LIMIT 1;

          SELECT cmt.ID INTO v_CurrDefault FROM
                  CurrencyMainTb cmt
          WHERE
                  cmt.IsDefault = 1
 ORDER BY cmt.ID DESC LIMIT 1;

          SELECT tu.USID INTO v_UserID FROM
                  TB_Users tu
          WHERE
                  tu.AccID = p_SafeID
 ORDER BY tu.USID DESC LIMIT 1;
SELECT at.AccName INTO v_AccName FROM AccountsTb at WHERE at.AccID=p_AccIDFrom;






          IF EXISTS (SELECT
                        1
                FROM
                        EMPORCUSTWITHDRAWALTB AS a
                WHERE
                        a.Code = p_Code)  AND p_IsUpdate=0
             THEN
              SET p_MSG = 2;
              SET p_MSGBOX = N'عذراً، رقم الكود محجوز مسبقاً. الرجاء إعادة المحاولة.';
              ROLLBACK;
              LEAVE proc;
            END IF;



          IF p_TypeID = 5 AND p_IsUpdate=0
             THEN

              SET v_AccVal = Account_GetAccVal(p_AccIDFrom, p_CurrencyFrom, 0);
              SELECT CASE                                           WHEN at.AccDmType = 0                                             THEN                                             1                                           ELSE                                           at.CanDebit                                   END, CASE                                           WHEN at.AccDmType = 0                                             THEN                                             0                                           ELSE                                           IFNULL(at.IsLimited, 0)                                   END, IFNULL(at.LimitedVal, 0) INTO v_CanDEpit, v_IsLimted, v_LimtedVal FROM
                      AccountsTb at
              WHERE
                      AccID = p_AccIDFrom;




              IF v_CanDEpit = 0
                AND
                p_WDVAL > v_AccVal
                AND
                p_OPType<>90
                 THEN
                  SET p_MSG = 0;
                  SET p_MSGBOX = N'عذراً، القيمة المراد سحبها أكبر من الرصيد المتاح.';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF v_CanDEpit = 1
                AND
                v_IsLimted = 1
                AND
                p_WDVAL > (v_LimtedVal + v_AccVal)
                 THEN
                  SET p_MSG = 0;
                  SET p_MSGBOX = N'القيمة المراد تحويلها أكبر من السقف المسموح به.';
                  ROLLBACK;
                  LEAVE proc;
                END IF;

              IF EXISTS (SELECT
                            1
                    FROM
                            EmployeeTb et
                    WHERE
                            et.AccID = p_AccIDFrom
                            AND et.IsResignation = 1
                            AND et.ResignatoinDate > NOW())
                 THEN
                  IF p_WDVAL > v_AccVal
                     THEN
                      SET p_MSG = 0;
                      SET p_MSGBOX = N'عذراً، الموظف لديه طلب استقالة مسجّل، ولا يحق له السحب بما يتجاوز رصيده الحالي. يرجى مراجعة الإدارة لمزيد من التفاصيل.';
                      ROLLBACK;
                      LEAVE proc;
                    END IF;
                END IF;
            END IF;




          IF p_IsUpdate=0
           THEN
          INSERT INTO EMPORCUSTWITHDRAWALTB
                (
                  Code,
                  InsertDate,
                  EMPID,
                  WDVAL,
                  SafeID,
                  IsActive,
                  TypeID,
                  EMPORCUSTWITHDRAWALTB.CODEID,
                  BranchID,
                  DPSVAL,
                  Notes,
                  CurrencyFrom,
                  PaidFor,
                  Phone,
                  IDNo,
                  parentCode
                )
          VALUES
                  (
                    p_Code, NOW(), p_AccIDFrom, p_WDVAL, p_SafeID, 1, p_TypeID, p_CODEID, p_BranchID, p_DPSVAL, p_Notes, p_CurrencyFrom, p_PaidFor, p_Phone, p_IDNo,p_AccParent
                  );
END IF;
          IF p_TypeID = 5
             THEN
            IF p_IsUpdate = 0
             THEN
                SET v_MovementType2  = CONCAT(N'صرف من حساب', SPACE(1), IFNULL(v_AccName,N''));
                SET v_MovementType = CONCAT(N'صُرفت لصالح ', SPACE(1), IFNULL(p_PaidFor,N''), N' هـ/ ', SPACE(1), IFNULL(p_Phone,N''));
             ELSE
                SET v_MovementType  = N'معالجة خطأ في عملية سحب من حساب';
                SET v_MovementType2 = N'معالجة خطأ في عملية سحب من حساب';
            END IF;

              IF v_IsDefault = 1
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
                          MovementType,
                          CurrencyID,
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note
                        )
                  VALUES
                          (
                            v_UserID, CASE WHEN p_IsUpdate = 0 THEN p_WDVAL ELSE 0.000 END, CASE WHEN p_IsUpdate = 0 THEN 0.000 ELSE p_WDVAL END, NOW(), p_Code, 1, 5, p_OPType, p_BranchID, p_AccIDFrom, p_SafeID, v_MovementType, p_CurrencyFrom, p_Notes, v_MovementType2, p_Notes
                          );
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
                          MovementType,
                          CurrencyID,
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note
                        )
                  VALUES
                          (
                            v_UserID, CASE WHEN p_IsUpdate = 0 THEN 0.000 ELSE p_WDVAL END, CASE WHEN p_IsUpdate = 0 THEN p_WDVAL ELSE 0.000 END, NOW(), p_Code, 1, 5, p_OPType, p_BranchID,p_SafeID, p_AccIDFrom,  v_MovementType, p_CurrencyFrom, p_Notes, v_MovementType2, p_Notes
                          );
                END IF;
              IF v_IsDefault <> 1
                 THEN


                  SELECT a.EMFtherPRint, a.ACCFPrintCridet INTO v_FAtherPERints, v_ACCFPrintCridet FROM
                          CurrencyMainTb AS a
                  WHERE
                          a.ID = p_CurrencyFrom
 ORDER BY a.ID DESC LIMIT 1;
                  SELECT AccID INTO v_FirstCurrencyd FROM
                          AccountsTb AS a
                  WHERE
                          a.AccParent = v_FAtherPERints
                          AND a.BranchID = p_BranchID;
                  SELECT AccID INTO v_ACCFPrintCridetACC FROM
                          AccountsTb AS a
                  WHERE
                          a.AccParent = v_ACCFPrintCridet
                          AND a.BranchID = p_BranchID
                          AND a.AccActive = 1;























































                        INSERT INTO ExSyAccountsCurrency_AccSafeActivityTb
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
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note,
                          ValType,
                          CountryID
                        )
                  VALUES
                          (
                            v_UserID, CASE WHEN p_IsUpdate=0 then 0.000 ELSE p_WDVAL END,
                    CASE WHEN p_IsUpdate=0 then p_WDVAL ELSE 0.000 END,
                            NOW(), p_Code, 5, 38,  p_BranchID, p_AccIDFrom, 225, v_MovementType, p_CurrencyFrom, p_Notes,
                            v_MovementType2, p_Notes,1,0
                          );

                       INSERT INTO ExSyAccountsCurrency_AccSafeActivityTb
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
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note,
                          ValType,
                          CountryID
                        )
                  VALUES
                          (
                            v_UserID, CASE WHEN p_IsUpdate=0 then p_WDVAL ELSE 0.000 END,
                    CASE WHEN p_IsUpdate=0 then 0.000 ELSE p_WDVAL END,
                            NOW(), p_Code, 5, 38,  p_BranchID, p_SafeID, 225, v_MovementType, p_CurrencyFrom, p_Notes,
                            v_MovementType2, p_Notes,0,0
                          );

                END IF;
            END IF;
                    IF p_TypeID = 7
             THEN
                        IF p_IsUpdate = 0
             THEN
                SET v_MovementType2  = CONCAT(N'إيداع في حساب', SPACE(1), IFNULL(v_AccName,N''));
                SET v_MovementType = CONCAT(N'دفعت من طرف', SPACE(1), IFNULL(p_PaidFor,N''), N' هـ/ ', SPACE(1), IFNULL(p_Phone,N''));
             ELSE
                SET v_MovementType  = N'معالجة خطأ في عملية إيداع في حساب';
                SET v_MovementType2 = N'معالجة خطأ في عملية إيداع في حساب';
            END IF;

              IF v_IsDefault = 1
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
                          MovementType,
                          CurrencyID,
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note
                        )
                  VALUES
                          (
                            v_UserID,CASE WHEN p_IsUpdate = 0 THEN 0.000 ELSE p_WDVAL END, CASE WHEN p_IsUpdate = 0 THEN p_WDVAL ELSE 0.000 END,  NOW(), p_Code, 1, 7, 40, p_BranchID, p_AccIDFrom, p_SafeID, v_MovementType, v_CurrDefault, p_Notes, v_MovementType2, p_Notes
                          );
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
                          MovementType,
                          CurrencyID,
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note
                        )
                  VALUES
                          (
                            v_UserID,CASE WHEN p_IsUpdate = 0 THEN p_WDVAL ELSE 0.000 END, CASE WHEN p_IsUpdate = 0 THEN 0.000 ELSE p_WDVAL END,  NOW(), p_Code, 1, 7, 40, p_BranchID,p_SafeID, p_AccIDFrom,  v_MovementType, v_CurrDefault, p_Notes, v_MovementType2, p_Notes
                          );
                END IF;
              IF v_IsDefault <> 1
                 THEN


                  SELECT a.EMFtherPRint, a.ACCFPrintCridet INTO v_FAtherPERints, v_ACCFPrintCridet FROM
                          CurrencyMainTb AS a
                  WHERE
                          a.ID = p_CurrencyFrom
 ORDER BY a.ID DESC LIMIT 1;
                  SELECT AccID INTO v_FirstCurrencyd FROM
                          AccountsTb AS a
                  WHERE
                          a.AccParent = v_FAtherPERints
                          AND a.BranchID = p_BranchID;
                  SELECT AccID INTO v_ACCFPrintCridetACC FROM
                          AccountsTb AS a
                  WHERE
                          a.AccParent = v_ACCFPrintCridet
                          AND a.BranchID = p_BranchID
                          AND a.AccActive = 1;






















































                        INSERT INTO ExSyAccountsCurrency_AccSafeActivityTb
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
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note,
                          ValType,
                          CountryID
                        )
                  VALUES
                          (
                            v_UserID,CASE WHEN p_IsUpdate=0 then p_WDVAL ELSE 0.000 END, CASE WHEN p_IsUpdate=0 then 0.000 ELSE p_WDVAL END,
                            NOW(), p_Code, 7, 40,  p_BranchID, p_AccIDFrom, 225, v_MovementType, p_CurrencyFrom, p_Notes,
                            v_MovementType2, p_Notes,1,0
                          );

                       INSERT INTO ExSyAccountsCurrency_AccSafeActivityTb
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
                          `Description`,
                          SafeIDMovement,
                          AccSafeActivityTb.Note,
                          ValType,
                          CountryID
                        )
                  VALUES
                          (
                            v_UserID,CASE WHEN p_IsUpdate=0 then 0.000 ELSE p_WDVAL END, CASE WHEN p_IsUpdate=0 then p_WDVAL ELSE 0.000 END,
                            NOW(), p_Code, 7, 40,  p_BranchID, p_SafeID, 225, v_MovementType, p_CurrencyFrom, p_Notes,
                            v_MovementType2, p_Notes,0,0
                          );

                END IF;
            END IF;


IF p_IsUpdate=1
 THEN
	UPDATE EMPORCUSTWITHDRAWALTB SET IsActive=0 WHERE Code=p_Code;
END IF;





      COMMIT;
      SET p_MSG = 1;
  END;
END
//
DELIMITER ;

DROP PROCEDURE IF EXISTS `AccSafeActivityTb_Statment`;
DELIMITER //
CREATE PROCEDURE `AccSafeActivityTb_Statment`(IN `p_ISID` LONGTEXT, IN `p_D1` DATE, IN `p_D2` DATE, IN `p_ShearchType` TINYINT(1), IN `p_BaseType` INT)
BEGIN
SET p_BaseType = IFNULL(p_BaseType, 0);
BEGIN





    IF p_BaseType = 0
     THEN

        IF p_ShearchType = 1
         THEN
            SELECT
                a.ID, a.ISID, a.InsertDate,
                a.Credit, a.Debit,
                a.SafeIDMovement, a.MovementType,
                a.TypeID, a.OperationTypeID,
                a.Note,
                b.AccName AS AccFrom,
                a.AccIDFrom,
                a.AccIDTo,
                c.AccName AS AccTo,
                b.AccCode,
                a.AccBranchID,
                d.BName,
                a.CurrencyID,
                a.IsActive,
                a.DailyClosed,
                a.IsBank
            FROM ExSyAccounts_AccSafeActivityTb a
            LEFT JOIN AccountsTb b ON a.AccIDFrom = b.AccID
            LEFT JOIN AccountsTb c ON a.AccIDTo = c.AccID
            LEFT JOIN CoBranch d ON a.AccBranchID = d.ID
            WHERE a.IsActive = 1
            AND a.ISID = p_ISID;
         ELSE
            SELECT
                a.ID, a.ISID, a.InsertDate,
                a.Credit, a.Debit,
                a.SafeIDMovement, a.MovementType,
                a.TypeID, a.OperationTypeID,
                a.Note,
                b.AccName AS AccFrom,
                a.AccIDFrom,
                a.AccIDTo,
                c.AccName AS AccTo,
                b.AccCode,
                a.AccBranchID,
                d.BName,
                a.CurrencyID,
                a.IsActive,
                a.DailyClosed,
                a.IsBank
            FROM ExSyAccounts_AccSafeActivityTb a
            LEFT JOIN AccountsTb b ON a.AccIDFrom = b.AccID
            LEFT JOIN AccountsTb c ON a.AccIDTo = c.AccID
            LEFT JOIN CoBranch d ON a.AccBranchID = d.ID
            WHERE a.IsActive = 1
            AND a.InsertDate >= p_D1
            AND a.InsertDate < DATE_ADD(p_D2, INTERVAL 1 DAY);
        END IF;

     ELSE

        IF p_ShearchType = 1
         THEN
            SELECT
                a.ID, a.ISID, a.InsertDate,
                a.Credit, a.Debit,
                a.SafeIDMovement, a.MovementType,
                a.TypeID, a.OperationTypeID,
                a.Note,
                b.AccName AS AccFrom,
                a.AccIDFrom,
                a.AccIDTo,
                c.AccName AS AccTo,
                b.AccCode,
                a.AccBranchID,
                d.BName,
                a.CurrencyID,
                a.IsActive,
                a.DailyClosed
            FROM ExSyAccountsCurrency_AccSafeActivityTb a
            LEFT JOIN AccountsTb b ON a.AccIDFrom = b.AccID
            LEFT JOIN AccountsTb c ON a.AccIDTo = c.AccID
            LEFT JOIN CoBranch d ON a.AccBranchID = d.ID
            WHERE a.IsActive = 1
            AND a.ISID = p_ISID;
         ELSE
            SELECT
                a.ID, a.ISID, a.InsertDate,
                a.Credit, a.Debit,
                a.SafeIDMovement, a.MovementType,
                a.TypeID, a.OperationTypeID,
                a.Note,
                b.AccName AS AccFrom,
                a.AccIDFrom,
                a.AccIDTo,
                c.AccName AS AccTo,
                b.AccCode,
                a.AccBranchID,
                d.BName,
                a.CurrencyID,
                a.IsActive,
                a.DailyClosed
            FROM ExSyAccountsCurrency_AccSafeActivityTb a
            LEFT JOIN AccountsTb b ON a.AccIDFrom = b.AccID
            LEFT JOIN AccountsTb c ON a.AccIDTo = c.AccID
            LEFT JOIN CoBranch d ON a.AccBranchID = d.ID
            WHERE a.IsActive = 1
            AND a.InsertDate >= p_D1
            AND a.InsertDate < DATE_ADD(p_D2, INTERVAL 1 DAY);
        END IF;

    END IF;

END;
END
//
DELIMITER ;

DROP PROCEDURE IF EXISTS `SalaryCalculation_LoadToBankPortfolio`;
DELIMITER //
CREATE PROCEDURE `SalaryCalculation_LoadToBankPortfolio`(IN `p_BankID` INT, IN `p_BBranchID` INT, IN `p_SALARYYEAR` INT, IN `p_SALARYMONTH` INT, INOUT `p_TotalSalary` DECIMAL(18,3))
proc: BEGIN
SET p_BBranchID = IFNULL(p_BBranchID, 0);
SET p_BankID = IFNULL(p_BankID, 0);
BEGIN



    IF p_SALARYYEAR IS NULL
      OR
      p_SALARYMONTH IS NULL
       THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='خطأ: يجب إدخال سنة الراتب وشهر الراتب.';
        LEAVE proc;
      END IF;













    SELECT
            B.CODE AS `codefromem`,
            MAX(b.AccOwner)    AS `الاسم`,
            b.BankNo           AS `رقم الحساب`,
            SUM(a.SALARYTOTAL) AS `القيمة`,

            c.BranchName AS `BankName_from`
    FROM
            SalaryCalculationTb a
        INNER JOIN
          EmployeeTb b
            ON a.EMPID = b.ID
        LEFT JOIN
          ExBBranchTb c
            ON b.BBranchID = c.ID



    WHERE
            a.SALARYEAR = p_SALARYYEAR
            AND a.SALARYMONTH = p_SALARYMONTH
            AND b.BankNo IS NOT NULL
            AND (   p_BankID = 0
                    OR b.BankID = p_BankID)


            AND a.BankSalaryCalc = 1
            AND b.BankSalaryCalc = 1
            AND a.IsIndivdual = 0
            AND b.BankNo IS NOT NULL
            AND b.BankNo <> ''
            AND b.AccOwner IS NOT NULL
            AND b.AccOwner <> ''
            AND a.SALARYTOTAL > 0
    GROUP BY
            b.BankNo,
            b.EMPNAME,
            B.CODE,
            c.BranchName;

    SELECT IFNULL(SUM(a.SALARYTOTAL), 0) INTO p_TotalSalary FROM
            SalaryCalculationTb a
        INNER JOIN
          EmployeeTb b
            ON a.EMPID = b.ID
    WHERE
            a.SALARYEAR = p_SALARYYEAR
            AND a.SALARYMONTH = p_SALARYMONTH
            AND (   p_BankID = 0
                    OR b.BankID = p_BankID)


            AND b.BankSalaryCalc = 1
            AND a.BankSalaryCalc=1
            AND a.IsIndivdual = 0
            AND b.BankNo IS NOT NULL
            AND b.BankNo <> ''
            AND b.AccOwner IS NOT NULL
            AND b.AccOwner <> ''
            AND a.SALARYTOTAL > 0;

    IF ROW_COUNT() = 0
       THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='تنبيه: لا توجد بيانات رواتب مطابقة للمعايير المحددة.';
        LEAVE proc;
      END IF;
  END;
END
//
DELIMITER ;

DROP PROCEDURE IF EXISTS `ACCOUNTSTB_selectmax`;
DELIMITER //
CREATE PROCEDURE `ACCOUNTSTB_selectmax`(IN `p_fatherParent` DECIMAL(18,0), IN `p_AccType` INT)
BEGIN
DECLARE v_code DECIMAL(18, 0);
DECLARE v_accLine INT;
DECLARE v_IDCode bigINT;
SET v_accLine = GetAccline(p_fatherParent);

	IF p_AccType = 0
		 THEN
			IF p_fatherParent = 0
				 THEN
SET v_IDCode = `AccountsTb_GetIDCode`(p_fatherParent, p_AccType);
SET v_code = CAST(v_IDCode AS CHAR);
				END IF;
			IF p_fatherParent > 0
				 THEN
SET v_IDCode = CAST(`AccountsTb_GetIDCode`(p_fatherParent, p_AccType) AS CHAR);
SET v_code = CONCAT(CAST(p_fatherParent AS CHAR), '0', CAST(v_IDCode AS CHAR));
				END IF;
		END IF;
	IF p_AccType = 1
		 THEN
SET v_IDCode = CAST(`AccountsTb_GetIDCode`(p_fatherParent, p_AccType) AS CHAR);
SET v_code = CONCAT(CAST(p_fatherParent AS CHAR), '0', CAST(v_IDCode AS CHAR));
		END IF;
	SELECT
			v_code	 AS code,
			v_accLine AS accLine,
			v_IDCode	 AS IDcode;
END
//
DELIMITER ;

-- Re-grant EXECUTE to the app user (DROP+CREATE wiped per-object grants; STATUS.md §8)
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`Companies_Crud` TO `exchange_app`@`%`;
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`EMPLOYEETB_Insert` TO `exchange_app`@`%`;
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`EMPORCUSTWITHDRAWALTB_Insert` TO `exchange_app`@`%`;
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`AccSafeActivityTb_Statment` TO `exchange_app`@`%`;
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`SalaryCalculation_LoadToBankPortfolio` TO `exchange_app`@`%`;
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`ACCOUNTSTB_selectmax` TO `exchange_app`@`%`;
FLUSH PRIVILEGES;

-- Data repair (already applied via exchange_app 2026-07-27; idempotent):
UPDATE `exchangesys2026`.`Companies` SET IsActive = 1 WHERE IsActive IS NULL;
