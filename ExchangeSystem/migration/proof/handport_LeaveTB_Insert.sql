-- =====================================================================================
-- Hand-port: LeaveTB_Insert  (two T-SQL forward jumps -> one flag)
--
-- TWO jumps, both targeting the SAME label:
--   end of IF @IsUpdate = 1  -> GOTO Calc
--   end of IF @IsUpdate = 2  -> GOTO Calc
--   label Calc: is the FIRST statement inside IF @IsUpdate = 3
--
-- Because the label is first in its block there is NOTHING to skip, so no IF v_goto = 0 wrapper is
-- needed - only the guard has to admit arrival via the flag:
--   GOTO Calc          ->  SET v_goto_Calc = 1;   (then fall out of the branch)
--   IF @IsUpdate = 3   ->  IF (p_IsUpdate = 3 OR v_goto_Calc = 1)
--   Calc:              ->  removed
--
-- Both jumps are the LAST statement in their branch (only block ENDs follow), and the statement
-- before each already ends in ';', so nothing else needed terminating.
--
-- !! Writes to AccSafeActivityTb (unpaid-leave deduction) - verify with one leave save per
-- !! IsUpdate path (1, 2, 3) against SQL Server.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `LeaveTB_Insert`;
DELIMITER $$
CREATE PROCEDURE `LeaveTB_Insert`(IN `p_Code` VARCHAR(50), IN `p_EMPID` INT, IN `p_BranchID` INT, IN `p_DateFrom` DATE, IN `p_DaysNumber` INT, IN `p_IsDiscount` INT, IN `p_VacationType` INT, IN `p_LeaveType` INT, IN `p_LeaveTypeID` INT, IN `p_Notes` LONGTEXT, IN `p_AbsenceID` INT, IN `p_IsAbsence` TINYINT(1), IN `p_ABDays` INT, IN `p_DiscountVal` DECIMAL(12,3), IN `p_IsActive` TINYINT(1), IN `p_IsUpdate` INT, INOUT `p_MSGSTatues` INT, INOUT `p_MsgBox` LONGTEXT)
proc: BEGIN
DECLARE v_goto_Calc INT DEFAULT 0;
DECLARE v_ISExist INT;
DECLARE v_DaysCount INT;
DECLARE v_SalaryVal DECIMAL(12, 3);
DECLARE v_salarPerday DECIMAL(12, 3);
DECLARE v_DaysInMonth INT DEFAULT DAY(EOMONTH(NOW()));
DECLARE v_DailySalary DECIMAL(12, 0);
DECLARE v_DateTo       DATE;
DECLARE v_NDiscountVal DECIMAL(12, 3);
DECLARE v_IsEnd BIT;
DECLARE v_IsAcepted SMALLINT;
DECLARE v_OldDaysNum INT;
DECLARE v_D1 DATE;
DECLARE v_D2 DATE;
DECLARE v_DateTo2 DATE;
DECLARE v_MovType LONGTEXT;
DECLARE v_MovType2 LONGTEXT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; CALL ERROR_PROC(); SET p_MSGSTatues = 0; END;
    START TRANSACTION;
SET v_DateTo = DATE_ADD(p_DateFrom, INTERVAL p_DaysNumber - 1 DAY);
SET v_DateTo2 = DATE_ADD(p_DateFrom, INTERVAL p_DaysNumber + p_ABDays DAY);
SET v_DaysCount = p_DaysNumber;
    SELECT SalaryVal INTO v_SalaryVal FROM
            EmployeeTb
    WHERE
            ID = p_EMPID;

    IF p_VacationType = 1
       THEN
SET v_NDiscountVal = CAST(v_SalaryVal / 30 AS DECIMAL(12, 0)) * v_DaysCount;
      END IF;

    SELECT EndLeave, IsAccepted, OverallLeaveDays INTO v_IsEnd, v_IsAcepted, v_OldDaysNum FROM
            LeaveTB
    WHERE
            Code = p_Code;

    SELECT DateFrom, DateTo INTO v_D1, v_D2 FROM
            LeaveTB
    WHERE
            EMPID = p_EMPID
            AND (   (DateFrom BETWEEN p_DateFrom AND v_DateTo)
                    OR (DateTo BETWEEN p_DateFrom AND v_DateTo));








    IF v_IsEnd = 1
       THEN
        SET p_MSGSTatues = 0;
        SET p_MsgBox = 'عذرا تم إنهاء هذه الإجازة مسبقا ولايمكن اجراء أي تعديل عليها';
        ROLLBACK;
        LEAVE proc;
      END IF;
    IF v_IsEnd = 1
       THEN
        SET p_MSGSTatues = 0;
        SET p_MsgBox = 'عذرا تم إنهاء هذه الإجازة مسبقا ولايمكن اجراء أي تعديل عليها';
        ROLLBACK;
        LEAVE proc;
      END IF;
    IF v_IsAcepted = 0
      AND
      p_IsUpdate <> 0
       THEN
        SET p_MSGSTatues = 0;
        SET p_MsgBox = 'عذرا لم يتم إعتماد هذه الإجازة بعد يرجى إعتمادها أولا ثم إتخاذ الإجراء';
        ROLLBACK;
        LEAVE proc;
      END IF;
    IF v_IsAcepted = 2
      AND
      p_IsUpdate <> 0
       THEN
        SET p_MSGSTatues = 0;
        SET p_MsgBox = 'عذرا تم رفض إعتماد هذه الإجازة من قبل الإدارة ولايمكن إتخاذ أي إجراء عليها';
        ROLLBACK;
        LEAVE proc;
      END IF;

    IF p_IsUpdate = 0
       THEN

        IF v_D1 IS NOT NULL
          AND
          v_D2 IS NOT NULL
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = CONCAT('عذرا الموظف لديه إجازة سابقة خلال الفترة من', CHAR(13), CAST(v_D1 AS CHAR), CHAR(13), 'إلى', CHAR(13), CAST(v_D2 AS CHAR), CHAR(13), 'الرجاء اختيار تاريخ خارج هذه الفترة');
            ROLLBACK;
            LEAVE proc;
          END IF;

        INSERT INTO LeaveTB
              (
                code,
                InsertDate,
                EMPID,
                BranchID,
                DateFrom,
                DateTo,
                VacationType,
                LeaveType,
                LeaveTypeID,
                Notes,
                AbsenceID,
                IsDiscount,
                IsAbsence,
                ABDays,
                DiscountVal,
                OverallLeaveDays,
                EndLeave,
                IsActive
              )
            SELECT
                    p_Code,
                    CAST(NOW() AS DATE),
                    p_EMPID,
                    p_BranchID,
                    p_DateFrom,
                    v_DateTo,
                    p_VacationType,
                    p_LeaveType,
                    p_LeaveTypeID,
                    p_Notes,
                    0,
                    0,
                    0,
                    0,
                    CASE                              WHEN p_LeaveType = 0                                THEN                                0.000                              WHEN p_LeaveType = 1                                THEN                                ROUND(((CAST(v_SalaryVal / 30 AS DECIMAL(12, 0)) * p_DaysNumber) / 5.0), 0) * 5                              WHEN p_LeaveType = 2                                THEN                                ROUND(((CAST(v_SalaryVal / 30 AS DECIMAL(12, 0)) * p_DaysNumber * 3) / 5.0), 0) * 5                      END,
                    v_DaysCount,
                    0,
                    p_IsActive;
      END IF;
    IF p_IsUpdate = 1
       THEN

        IF v_DateTo2 > NOW()
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = 'عذرا لم يحن موعد المباشرة بعد الرجاء إنهاء الإجازة في موعدها المحدد';
            ROLLBACK;
            LEAVE proc;
          END IF;

        IF p_IsAbsence = 1
           THEN
            IF p_AbsenceID = 2
               THEN
                UPDATE
                        LeaveTB
                SET
                        DiscountVal = DiscountVal + ROUND((p_DiscountVal / 5.0), 0) * 5,
                        OverallLeaveDays = OverallLeaveDays + p_ABDays,
                        ABDays = p_ABDays,
                        EndLeave = 1,
                        IsAbsence = p_IsAbsence,
                        AbsenceID = p_AbsenceID
                WHERE
                  Code = p_Code;
              END IF;
            IF p_AbsenceID = 0
               THEN

                UPDATE
                        LeaveTB
                SET
                        DiscountVal = DiscountVal + CASE                                                WHEN p_IsDiscount = 0                                                  THEN                                                  ROUND(((CAST(v_SalaryVal / 30 AS DECIMAL(12, 0)) * p_ABDays) / 5.0), 0) * 5                                                ELSE                                                0                                        END,
                        OverallLeaveDays = OverallLeaveDays + p_ABDays,
                        IsDiscount = p_IsDiscount,
                        ABDays = p_ABDays,
                        EndLeave = 1,
                        IsAbsence = p_IsAbsence,
                        AbsenceID = p_AbsenceID
                WHERE
                  Code = p_Code;
              END IF;
            IF p_AbsenceID = 1
               THEN
                UPDATE
                        LeaveTB
                SET
                        DiscountVal = DiscountVal + ROUND(((CAST(v_SalaryVal / 30 AS DECIMAL(12, 0)) * p_ABDays * 3) / 5.0), 0) * 5,
                        OverallLeaveDays = OverallLeaveDays + p_ABDays,
                        ABDays = p_ABDays,
                        EndLeave = 1,
                        IsAbsence = p_IsAbsence,
                        AbsenceID = p_AbsenceID
                WHERE
                  Code = p_Code;
              END IF;
          END IF;
        SET v_goto_Calc = 1;   -- jump replaced by flag

      END IF;
    IF p_IsUpdate = 2
       THEN

        IF v_OldDaysNum < p_DaysNumber
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = 'عذرا لا يمكن أن يكون عدد الأيام الجديد أكبر من عدد الأيام السابق';
            ROLLBACK;
            LEAVE proc;
          END IF;

        IF v_DateTo >= NOW()
           THEN
            SET p_MSGSTatues = 0;
            SET p_MsgBox = 'عذرا لم يحن موعد المباشرة بعد الرجاء إنهاء الإجازة في موعدها المحدد';
            ROLLBACK;
            LEAVE proc;
          END IF;

        UPDATE
                LeaveTB
        SET
                EndLeave = 1, IsDiscount = CASE                                      WHEN p_LeaveType = 1                                        THEN                                        0                                      ELSE                                      1                              END, OverallLeaveDays = p_DaysNumber
        WHERE
          Code = p_Code;

        IF v_OldDaysNum <> p_DaysNumber
           THEN
            UPDATE
                    LeaveTB
            SET
                    DiscountVal = CASE                                           WHEN p_LeaveType = 0                                             THEN                                             0.000                                           WHEN p_LeaveType = 1                                             THEN                                             ROUND(((CAST(v_SalaryVal / 30 AS DECIMAL(12, 0)) * p_DaysNumber) / 5.0), 0) * 5                                   END
            WHERE
              Code = p_Code;
          END IF;

        SET v_goto_Calc = 1;   -- jump replaced by flag
      END IF;
    IF (p_IsUpdate = 3 OR v_goto_Calc = 1)
       THEN
        -- (label Calc removed; reached via v_goto_Calc)

SET v_MovType  = CONCAT('خصميات شهر', ' - ', CAST(MONTH(NOW()) AS CHAR), ' - ', CAST(YEAR(NOW()) AS CHAR));
SET v_MovType2 = 'خصم إجازة غير مدفوعة';
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
                Note,
                CurrencyID,
                DailyClosed,
                SafeIDDailyClose
              )
            SELECT
                    1,
                    b.DiscountVal,
                    0.000,
                    NOW(),
                    CONCAT(CAST(A.BranchID AS CHAR), ' - ', '54', ' - ', CAST(A.ID AS CHAR)) AS ISID,
                    1,
                    54,
                    89,
                    A.BranchID,
                    A.AccID,
                    (   SELECT
                                AccID
                        FROM
                                AccountsTb
                        WHERE
                                AccParent = 4010209
                                AND AccountsTb.BranchID = A.BranchID),
                    1,
                    0,
                    v_MovType,
                    CASE                              WHEN b.LeaveType = 1                                THEN                                CONCAT(v_MovType2, ' لمدة ', CAST(b.OverallLeaveDays AS CHAR)) + (CASE                                        WHEN b.OverallLeaveDays > 11                                          THEN                                          ' يوم'                                        ELSE                                        ' أيام'                                END)                              WHEN b.LeaveType = 0                                THEN                                CONCAT('غياب لمدة ', CAST(b.ABDays AS CHAR), ' أيام')                              WHEN b.LeaveType = 2                                THEN                                CONCAT('غياب بدون اذن  لمدة ', CAST(b.OverallLeaveDays AS CHAR), ' أيام')                      END,
                    1,
                    0,
                    0
            FROM
                    EmployeeTb AS A
                LEFT JOIN
                  LeaveTB b
                    ON a.ID = b.EMPID
            WHERE
                    b.DiscountVal > 0.000
                    AND IsCalculated = 0
                    AND b.IsAccepted = 1
                    AND b.Code = p_Code;


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
                SafeIDDailyClose
              )
            SELECT
                    1,
                    0.000,
                    b.DiscountVal,
                    NOW(),
                    CONCAT(CAST(A.BranchID AS CHAR), ' - ', '54', ' - ', CAST(A.ID AS CHAR)) AS ISID,
                    1,
                    54,
                    89,
                    A.BranchID,
                    (   SELECT
                                AccID
                        FROM
                                AccountsTb
                        WHERE
                                AccParent = 4010209
                                AND AccountsTb.BranchID = A.BranchID),
                    A.AccID,
                    1,
                    0,
                    v_MovType,
                    1,
                    0,
                    0
            FROM
                    EmployeeTb AS A
                LEFT JOIN
                  LeaveTB b
                    ON a.ID = b.EMPID
            WHERE
                    b.DiscountVal > 0.000
                    AND IsCalculated = 0
                    AND b.IsAccepted = 1
                    AND b.Code = p_Code;
        UPDATE
                LeaveTB
        SET
                IsCalculated = 1
        WHERE
          IsCalculated = 0
          AND IsAccepted = 1
          AND Code = p_Code;

      END IF;
    COMMIT;
    SET p_MSGSTatues = 1;
END
$$
DELIMITER ;
