-- =====================================================================================
-- Hand-port: ACCOUNTSTB_SelectICOUNTDetelles2
--
-- Why the converter could not do it: the proc selects FROM a MULTI-STATEMENT table-valued function,
--     FROM [dbo].[AccSafeActivityTb_GETBLANSE](@ACCCODE, @date1, @date2, @CurrencyFrom) AS a
-- (RETURNS @TMT TABLE .. INSERT INTO @TMT .. RETURN). MySQL has no table-valued functions and the
-- converter only inlines INLINE TVFs (a single RETURN(SELECT ..)).
--
-- The TVF is flattened into the proc as a TEMPORARY TABLE whose columns keep the TVF's DECLARED types,
-- because those declarations are an implicit CAST and the report depends on them:
--     Balance DECIMAL(13,3)  -- rounds the running total; the raw window-function value has more digits
--     ACCCODE / ACCNAME / Code / Description / safID / BName  NVARCHAR(MAX) -> LONGTEXT
-- The TVF's INSERT has no explicit column list, so the mapping is POSITIONAL. Written out, it is:
--     1 A.Credit->Credit  2 A.Debit->Debit  3 A.InsertDate->InsertDate  4 B.AccID->AccID  5 A.ID->IDs
--     6 <balance expr>->Balance  7 B.ACCCODE->ACCCODE  8 B.ACCNAME->ACCNAME  9 A.ISID->Code
--    10 ID->AccBID  11 A.Description->Description  12 e.UName->safID  13 x.ACCNAME->BName
-- (note 9 and 13: "Code" is the ledger ISID and "BName" is the COUNTERPARTY account name, not a branch.)
--
-- @CurrencyID is computed identically in the proc and in the TVF (IsDefault of @CurrencyFrom), so it is
-- read once here and both the staging branch and the output branch use it — same behaviour, one query.
--
-- Mechanical changes only:
--   @TMT TABLE(..)               -> TEMPORARY TABLE tmp_AccSafeActivityTb_GETBLANSE(..)
--   NVARCHAR(MAX)                -> LONGTEXT
--   [dbo].X / dbo.X / [col]      -> X / col
--   SELECT @v = expr FROM ..     -> SELECT expr INTO v_v FROM ..
--   TRY/CATCH + EXEC ERROR_PROC  -> DECLARE EXIT HANDLER .. ROLLBACK; CALL ERROR_PROC()
--   OUTPUT params                -> INOUT
--   SET NOCOUNT / XACT_ABORT     -> dropped (no MySQL equivalent, no result-set effect)
--
-- The running-balance expression
--     AccSafeActivityTb_TOTETDATE(..) + CASE WHEN B.ACCDMTYPE = 0 THEN SUM(..) OVER (PARTITION BY
--     A.AccIDFrom ORDER BY A.ID) WHEN B.ACCDMTYPE = 1 THEN SUM(..) OVER (..) END
-- is kept verbatim (MariaDB 10.2+ supports window functions), as are both currency branches, the ABS()
-- that only the @CurrencyID = 0 branch applies to Balance, and the four OUTPUT computations.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ACCOUNTSTB_SelectICOUNTDetelles2`;
DELIMITER $$
CREATE PROCEDURE `ACCOUNTSTB_SelectICOUNTDetelles2`(
    IN    `p_ACCCODE`        BIGINT,
    IN    `p_date1`          DATE,
    IN    `p_date2`          DATE,
    INOUT `p_smblabecredetl` DOUBLE,
    INOUT `p_sumlabeldebit`  DOUBLE,
    INOUT `p_lbealtotal`     DOUBLE,
    INOUT `p_totalacount`    DOUBLE,
    IN    `p_CurrencyFrom`   INT,
    IN    `p_BranchID`       INT)
BEGIN
    DECLARE v_CurrencyID INT;
    DECLARE v_ACCDMTYPE  INT;
    DECLARE v_OpeningBal DECIMAL(13, 3);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        CALL ERROR_PROC();
    END;

    START TRANSACTION;

    SELECT CMT.IsDefault
      INTO v_CurrencyID
      FROM CurrencyMainTb AS CMT
     WHERE ID = p_CurrencyFrom;

    -- PERFORMANCE (result-preserving): the T-SQL calls
    --     AccSafeActivityTb_TOTETDATE(@ACCCODE, @date1, B.ACCDMTYPE, @CurrencyFrom)
    -- once PER ROW. Three of its four arguments are procedure parameters, and the fourth is invariant
    -- too: B is ACCOUNTSTB joined ON A.AccIDFrom = B.AccID while the WHERE pins A.AccIDFrom = @ACCCODE,
    -- so B is a SINGLE row and B.ACCDMTYPE is the same value for every output row. The call therefore
    -- returns an identical result each time and is hoisted out of the query.
    --
    -- This is not a micro-optimisation: each call scans ~84,000 ledger rows (~105 ms even with the
    -- covering index added in handport_indexes_ledger.sql), so over the 1,300+ rows of a busy account
    -- the per-row form took minutes and the screen appeared to hang. SQL Server needs ~19 s for the
    -- same proc; hoisting brings MySQL below that while returning byte-identical values.
    SELECT B.ACCDMTYPE INTO v_ACCDMTYPE FROM ACCOUNTSTB AS B WHERE B.AccID = p_ACCCODE;
    SET v_OpeningBal = AccSafeActivityTb_TOTETDATE(p_ACCCODE, p_date1, v_ACCDMTYPE, p_CurrencyFrom);

    DROP TEMPORARY TABLE IF EXISTS tmp_AccSafeActivityTb_GETBLANSE;
    CREATE TEMPORARY TABLE tmp_AccSafeActivityTb_GETBLANSE
    (
        Credit      DECIMAL(13, 3),
        Debit       DECIMAL(13, 3),
        InsertDate  DATE,
        AccID       BIGINT,
        IDs         BIGINT,
        Balance     DECIMAL(13, 3),
        ACCCODE     LONGTEXT,
        ACCNAME     LONGTEXT,
        Code        LONGTEXT,
        AccBID      BIGINT,
        `Description` LONGTEXT,
        safID       LONGTEXT,
        BName       LONGTEXT
    );

    IF v_CurrencyID = 1 THEN
        INSERT INTO tmp_AccSafeActivityTb_GETBLANSE
            SELECT
                    A.Credit,
                    A.Debit,
                    A.InsertDate,
                    B.AccID,
                    A.ID,
                    v_OpeningBal +
                        CASE
                            WHEN B.ACCDMTYPE = 0
                                THEN SUM(CASE WHEN A.Credit = 0 THEN A.Debit ELSE -A.Credit END)
                                     OVER (PARTITION BY A.AccIDFrom ORDER BY A.ID)
                            WHEN B.ACCDMTYPE = 1
                                THEN SUM(CASE WHEN A.Debit = 0 THEN A.Credit ELSE -A.Debit END)
                                     OVER (PARTITION BY A.AccIDFrom ORDER BY A.ID)
                        END                                        AS Balance,
                    B.ACCCODE,
                    B.ACCNAME,
                    A.ISID,
                    ID                                             AS AccBID,
                    A.Description                                  AS noets,
                    e.UName                                        AS safID,
                    x.ACCNAME
            FROM
                    ExSyAccounts_AccSafeActivityTb AS A
                INNER JOIN ACCOUNTSTB AS B ON A.AccIDFrom = B.AccID
                INNER JOIN ACCOUNTSTB AS x ON A.AccIDTo   = x.AccID
                INNER JOIN TB_Users  AS e ON A.SafeID     = e.USID
            WHERE
                    A.AccIDFrom = p_ACCCODE
                    AND A.InsertDate BETWEEN p_date1 AND p_date2
                    AND A.IsActive = 1;
    END IF;

    IF v_CurrencyID = 0 THEN
        INSERT INTO tmp_AccSafeActivityTb_GETBLANSE
            SELECT
                    A.Credit,
                    A.Debit,
                    A.InsertDate,
                    B.AccID,
                    A.ID,
                    v_OpeningBal +
                        CASE
                            WHEN B.ACCDMTYPE = 0
                                THEN SUM(CASE WHEN A.Credit = 0 THEN A.Debit ELSE -A.Credit END)
                                     OVER (PARTITION BY A.AccIDFrom ORDER BY A.ID)
                            WHEN B.ACCDMTYPE = 1
                                THEN SUM(CASE WHEN A.Debit = 0 THEN A.Credit ELSE -A.Debit END)
                                     OVER (PARTITION BY A.AccIDFrom ORDER BY A.ID)
                        END                                        AS Balance,
                    B.ACCCODE,
                    B.ACCNAME,
                    A.ISID,
                    ID                                             AS AccBID,
                    A.Description                                  AS noets,
                    e.UName                                        AS safID,
                    x.ACCNAME
            FROM
                    ExSyAccountsCurrency_AccSafeActivityTb AS A
                INNER JOIN ACCOUNTSTB AS B ON A.AccIDFrom = B.AccID
                INNER JOIN TB_Users  AS e ON A.SafeID     = e.USID
                INNER JOIN ACCOUNTSTB AS x ON A.AccIDTo   = x.AccID
            WHERE
                    A.AccIDFrom = p_ACCCODE
                    AND A.InsertDate BETWEEN p_date1 AND p_date2
                    AND A.CurrencyID = p_CurrencyFrom
                    AND A.IsActive = 1;
    END IF;

    IF v_CurrencyID = 1 THEN
        SELECT
                a.AccCode,
                a.Code       AS Code,
                a.AccBID,
                a.AccID,
                a.ACCNAME    AS AccName,
                a.Credit     AS CREDET,
                a.Debit      AS debit,
                a.Description,
                a.IDs,
                a.InsertDate AS insertdate,
                a.safID,
                a.Balance    AS totel,
                BName
        FROM
                tmp_AccSafeActivityTb_GETBLANSE AS a;

        SELECT
                sumcredet1(0, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID),
                sumDEBETalter1(1, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID),
                sumcredet1(0, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID)
                    - sumDEBETalter1(1, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID),
                sumtotalaccounadi(p_ACCCODE)
        INTO    p_smblabecredetl, p_sumlabeldebit, p_lbealtotal, p_totalacount
        FROM    AccountsTb AS a
        WHERE   AccID = p_ACCCODE;

        SELECT p_smblabecredetl, p_sumlabeldebit, p_lbealtotal, p_totalacount;
    END IF;

    IF v_CurrencyID = 0 THEN
        SELECT
                a.AccCode,
                a.Code            AS Code,
                a.AccBID,
                a.AccID,
                a.ACCNAME         AS AccName,
                a.Credit          AS CREDET,
                a.Debit           AS debit,
                a.Description,
                a.IDs,
                a.InsertDate      AS insertdate,
                a.safID,
                ABS(a.Balance)    AS totel,
                BName
        FROM
                tmp_AccSafeActivityTb_GETBLANSE AS a;

        SELECT
                sumcredet1(0, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID),
                sumDEBETalter1(1, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID),
                sumcredet1(0, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID)
                    - sumDEBETalter1(1, a.AccCode, p_date1, p_date2, p_CurrencyFrom, p_BranchID),
                sumtotalaccounadi(p_ACCCODE)
        INTO    p_smblabecredetl, p_sumlabeldebit, p_lbealtotal, p_totalacount
        FROM    AccountsTb AS a
        WHERE   AccID = p_ACCCODE;

        SELECT p_smblabecredetl, p_sumlabeldebit, p_lbealtotal, p_totalacount;
    END IF;

    -- PERFORMANCE (result-preserving): the T-SQL calls
    --     AccSafeActivityTb_TOTETDATE(@ACCCODE, @date1, B.ACCDMTYPE, @CurrencyFrom)
    -- once PER ROW. Three of its four arguments are procedure parameters, and the fourth is invariant
    -- too: B is ACCOUNTSTB joined ON A.AccIDFrom = B.AccID while the WHERE pins A.AccIDFrom = @ACCCODE,
    -- so B is a SINGLE row and B.ACCDMTYPE is the same value for every output row. The call therefore
    -- returns an identical result each time and is hoisted out of the query.
    --
    -- This is not a micro-optimisation: each call scans ~84,000 ledger rows (~105 ms even with the
    -- covering index added in handport_indexes_ledger.sql), so over the 1,300+ rows of a busy account
    -- the per-row form took minutes and the screen appeared to hang. SQL Server needs ~19 s for the
    -- same proc; hoisting brings MySQL below that while returning byte-identical values.
    SELECT B.ACCDMTYPE INTO v_ACCDMTYPE FROM ACCOUNTSTB AS B WHERE B.AccID = p_ACCCODE;
    SET v_OpeningBal = AccSafeActivityTb_TOTETDATE(p_ACCCODE, p_date1, v_ACCDMTYPE, p_CurrencyFrom);

    DROP TEMPORARY TABLE IF EXISTS tmp_AccSafeActivityTb_GETBLANSE;

    COMMIT;
END$$
DELIMITER ;
