SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP FUNCTION IF EXISTS `AccEMPACT_RetrunID`;
DELIMITER //
CREATE FUNCTION `AccEMPACT_RetrunID`(`p_SafeID` INT, `p_date1` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN
DECLARE v_mxID int;
DECLARE v_sum_creadet float;
DECLARE v_sum_totla float;

SELECT ID INTO v_mxID FROM AccEmpActivityTb as a where a.SafeID  = p_SafeID
and   a.InsertDate =  p_date1 LIMIT 1;









return v_mxID;
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`AccEMPACT_RetrunID` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Account_GetAccVal`;
DELIMITER //
CREATE FUNCTION `Account_GetAccVal`(`p_AccID` BIGINT, `p_CurrencyID` INT, `p_IsBank` TINYINT UNSIGNED) RETURNS decimal(18,3)
    DETERMINISTIC
BEGIN
DECLARE v_IsDefault BIT;
DECLARE v_AccDmType TINYINT;
DECLARE v_CanUseBankVal TINYINT;
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
    SELECT aa.IsDefault INTO v_IsDefault FROM
            CurrencyMainTb AS aa
    WHERE
            aa.ID = p_CurrencyID
 ORDER BY aa.ID DESC LIMIT 1;

    SELECT a.AccDmType, a.CanUseBankVal INTO v_AccDmType, v_CanUseBankVal FROM
            AccountsTb a
    WHERE
            a.AccID = p_AccID LIMIT 1;


IF v_CanUseBankVal=1
 THEN
	SET p_IsBank=2;
END IF;

    IF v_IsDefault = 1
       THEN
        SELECT CASE                                        WHEN v_AccDmType = 1                                          THEN                                          IFNULL(SUM(a.Credit), 0.000)                                        ELSE                                        IFNULL(SUM(a.Debit), 0.000)                                END INTO v_AccCreditVal FROM
                ExSyAccounts_AccSafeActivityTb AS a
            LEFT JOIN
              AccountsTb AS b
                ON a.AccIDFrom = b.AccID
        WHERE
                b.AccID = p_AccID
                AND (a.IsBank=p_IsBank OR p_IsBank=2 OR a.IsBank=3)
                AND a.IsActive=1;
        SELECT CASE                                       WHEN v_AccDmType = 0                                         THEN                                         IFNULL(SUM(a.Credit), 0.000)                                       ELSE                                       IFNULL(SUM(a.Debit), 0.000)                               END INTO v_AccdebitVal FROM
                ExSyAccounts_AccSafeActivityTb AS a
            LEFT JOIN
              AccountsTb AS b
                ON a.AccIDFrom = b.AccID
        WHERE
                b.AccID = p_AccID
                AND (a.IsBank=p_IsBank OR p_IsBank=2 OR a.IsBank=3)
                AND a.IsActive=1;
SET v_AccVal = v_AccCreditVal - v_AccdebitVal;
      END IF;
    IF v_IsDefault = 0
       THEN
        SELECT CASE                                        WHEN v_AccDmType = 1                                          THEN                                          IFNULL(SUM(a.Credit), 0.000)                                        ELSE                                        IFNULL(SUM(a.Debit), 0.000)                                END INTO v_AccCreditVal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            LEFT JOIN
              AccountsTb AS b
                ON a.AccIDFrom = b.AccID
        WHERE
                b.AccID = p_AccID
                AND CurrencyID = p_CurrencyID
                AND a.IsActive=1;

        SELECT CASE                                       WHEN v_AccDmType = 0                                         THEN                                         IFNULL(SUM(a.Credit), 0.000)                                       ELSE                                       IFNULL(SUM(a.Debit), 0.000)                               END INTO v_AccdebitVal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            LEFT JOIN
              AccountsTb AS b
                ON a.AccIDFrom = b.AccID
        WHERE
                b.AccID = p_AccID
                AND CurrencyID = p_CurrencyID
                AND a.IsActive=1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
      END IF;



    IF v_AccVal IS NULL
       THEN
SET v_AccVal = 0.000;
      END IF;
    RETURN v_AccVal;
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Account_GetAccVal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Account_GetAccVal_BuyCurrncey2026`;
DELIMITER //
CREATE FUNCTION `Account_GetAccVal_BuyCurrncey2026`(`p_AccID` BIGINT, `p_CrunnceyID` INT, `p_PriceType` INT, `p_CountryID` INT, `p_BranchID` INT, `p_BankID` BIGINT) RETURNS decimal(18,3)
    DETERMINISTIC
BEGIN
DECLARE v_IsDefault BIT;
DECLARE v_AccDmType TINYINT;
DECLARE v_CanUseBankVal TINYINT;
DECLARE v_IsBank INT;
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);

    SELECT a.AccDmType, a.CanUseBankVal INTO v_AccDmType, v_CanUseBankVal FROM
            AccountsTb a
    WHERE
            a.AccID = p_AccID LIMIT 1;


SET v_IsBank = 0;
          IF EXISTS (SELECT
                        1
                FROM
                        AccountsTb a
                WHERE
                        a.AccCode = p_BankID
                        AND a.AccParent = 10105)
             THEN
              SET v_IsBank = 1;
            END IF;

        SELECT CASE                                        WHEN v_AccDmType = 1                                          THEN                                          IFNULL(SUM(a.Credit), 0.000)                                        ELSE                                        IFNULL(SUM(a.Debit), 0.000)                                END INTO v_AccCreditVal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            LEFT JOIN
              AccountsTb AS b
                ON a.AccIDFrom = b.AccID
        WHERE
                b.AccID = p_AccID
                AND a.PriceType   = p_PriceType
                AND a.CurrencyID  = p_CrunnceyID
                AND a.AccBranchID = p_BranchID
                AND ((a.CountryID   = p_CountryID) OR (a.CountryID=0 AND p_PriceType=0))
                AND a.IsActive=1
                AND ((a.ParentFrom=p_BankID AND v_IsBank=1) OR (v_IsBank=0 AND a.IsBank=0));
        SELECT CASE                                       WHEN v_AccDmType = 0                                         THEN                                         IFNULL(SUM(a.Credit), 0.000)                                       ELSE                                       IFNULL(SUM(a.Debit), 0.000)                               END INTO v_AccdebitVal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            LEFT JOIN
              AccountsTb AS b
                ON a.AccIDFrom = b.AccID
        WHERE
                b.AccID = p_AccID
                AND a.PriceType   = p_PriceType
                AND a.CurrencyID  = p_CrunnceyID
                AND a.AccBranchID = p_BranchID
                AND ((a.CountryID   = p_CountryID) OR (a.CountryID=0 AND p_PriceType=0))
                AND a.IsActive=1
                AND ((a.ParentFrom=p_BankID AND v_IsBank=1) OR (v_IsBank=0 AND a.IsBank=0));
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
    IF v_AccVal IS NULL
       THEN
SET v_AccVal = 0.000;
      END IF;
    RETURN v_AccVal;
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Account_GetAccVal_BuyCurrncey2026` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Account_GetParentName`;
DELIMITER //
CREATE FUNCTION `Account_GetParentName`(`p_AccID` BIGINT) RETURNS varchar(80) CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci
    DETERMINISTIC
BEGIN
DECLARE v_AccType VARCHAR(80);
    SELECT CASE          WHEN EXISTS (SELECT 1 FROM CustomersTb c WHERE c.AccID = a.AccID) THEN '????'         WHEN EXISTS (SELECT 1 FROM CoBranch cb WHERE cb.CurrentAccID = a.AccID AND cb.BranchType=3) THEN '????'         WHEN EXISTS (SELECT 1 FROM CoBranch cb WHERE cb.CurrentAccID = a.AccID AND cb.BranchType=1) THEN '???'         WHEN EXISTS (SELECT 1 FROM AddPartnerTb apt WHERE apt.CurrentAcc = a.AccID ) THEN '????'         WHEN EXISTS (SELECT 1 FROM EmployeeTb et WHERE et.AccID = a.AccID ) THEN '????'         WHEN a.AccParent LIKE '2010110%' THEN '???? ?????'         WHEN a.AccParent LIKE '2010140%' THEN '????'         WHEN a.AccParent LIKE '2030201%' THEN '????? ?????'         WHEN a.AccParent LIKE '2010120%' THEN '?????'         ELSE '????'      END INTO v_AccType FROM AccountsTb a
    WHERE a.AccID=p_AccID LIMIT 1;
    IF v_AccType IS NULL
       THEN
SET v_AccType = '????';
      END IF;
    RETURN v_AccType;
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Account_GetParentName` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `AgentView_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `AgentView_GetNetTotal`(`p_AccName` BIGINT, `p_CurrencyID` INT, `p_ID` BIGINT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);

	SELECT IFNULL( SUM( a.Credit ), 0.000 )-IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
	WHERE a.IsActive=1

		  AND a.AccIDFrom=p_AccName
		  AND a.ID<p_ID;


	SELECT IFNULL( a.Credit, 0.000 ) INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
	WHERE a.IsActive=1

		  AND a.AccIDFrom=p_AccName
		  AND a.ID=p_ID LIMIT 1;
	SELECT IFNULL( a.Debit, 0.000 ) INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
LEFT JOIN
AccountsTb AS b
ON a.AccIDFrom=b.AccID
	WHERE a.IsActive=1
		  AND b.AccActive=1

		  AND a.AccIDFrom=p_AccName
		  AND a.ID=p_ID LIMIT 1;

	IF v_CurrenCredit>0.000
	AND v_CurrenDebit<=0.000
	 THEN
SET v_NetVal=(v_AccCreditVal+v_CurrenCredit);
	END IF;

	IF v_CurrenDebit>0.000
	 THEN
SET v_NetVal=v_AccCreditVal-v_CurrenDebit;
	END IF;

	RETURN IFNULL( v_NetVal, 0.000 );
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`AgentView_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Ass_sumcredet1`;
DELIMITER //
CREATE FUNCTION `Ass_sumcredet1`(`p_tybid` INT, `p_acccode` BIGINT, `p_date1` DATE, `p_date2` DATE, `p_CurrencyFrom` INT, `p_BranchID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_sumcredet FLOAT;
DECLARE v_CRUNSEFROM BIT;
DECLARE v_MainBranch INT;
DECLARE v_Ismain BIT;

    SELECT co.IsMain INTO v_Ismain FROM
            CoBranch AS co
    WHERE
            co.ID = p_BranchID LIMIT 1;
    SELECT co.ID INTO v_MainBranch FROM
            CoBranch AS co
    WHERE
            co.IsMain = 1 LIMIT 1;
    SELECT IsDefault INTO v_CRUNSEFROM FROM
            CurrencyMainTb AS CMT
    WHERE
            ID = p_CurrencyFrom
 ORDER BY CMT.ID DESC LIMIT 1;
    IF v_CRUNSEFROM = 1
       THEN
        IF v_Ismain = 1
           THEN
            SELECT IFNULL(SUM(Credit), 0) INTO v_sumcredet FROM
                    ExAssociationAct_AssActivityTb AS a
                INNER JOIN
                  AccountsTb AS b
                    ON b.AccID = a.AccIDFrom
            WHERE
                    b.AccCode = p_acccode
                    AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
                    AND a.IsActive = 1
                    AND (   a.CurrencyID = p_CurrencyFrom
                            OR a.CurrencyID = 13)
                    AND a.AccBranchID = v_MainBranch;
          END IF;

        IF v_Ismain <> 1
           THEN
            SELECT IFNULL(SUM(Credit), 0) INTO v_sumcredet FROM
                    ExAssociationAct_AssActivityTb AS a
                INNER JOIN
                  AccountsTb AS b
                    ON b.AccID = a.AccIDFrom
            WHERE
                    b.AccCode = p_acccode
                    AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
                    AND a.IsActive = 1
                    AND (   a.CurrencyID = p_CurrencyFrom
                            OR a.CurrencyID = 13)
                    AND a.AccBranchID = p_BranchID;
          END IF;

      END IF;



    RETURN v_sumcredet;
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Ass_sumcredet1` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Ass_sumDEBETalter1`;
DELIMITER //
CREATE FUNCTION `Ass_sumDEBETalter1`(`p_tybid` INT, `p_acccode` BIGINT, `p_date1` DATE, `p_date2` DATE, `p_CurrencyFrom` INT, `p_BranchID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_debit FLOAT;
DECLARE v_CurrencyID BIT;
DECLARE v_MainBranch INT;
DECLARE v_Ismain bit;
		SELECT co.ismain INTO v_Ismain FROM CoBranch as co where co.ID=p_BranchID LIMIT 1;
			SELECT co.id INTO v_MainBranch FROM CoBranch as co where co.ismain=1 LIMIT 1;

		SELECT IsDefault INTO v_CurrencyID FROM
				CurrencyMainTb AS CMT WHERE CMT.ID=p_CurrencyFrom
 ORDER BY CMT.ID DESC LIMIT 1;
		IF v_CurrencyID = 1
			 THEN

				IF p_tybid = 0
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExAssociationAct_AssActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccParent = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13);

					END IF;
				IF p_tybid = 1
					 THEN
					if v_Ismain=1
			 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
							ExAssociationAct_AssActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
								AND a.AccBranchID=v_MainBranch;
					END IF;
					if v_Ismain<>1
			 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
							ExAssociationAct_AssActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
								AND a.AccBranchID=p_BranchID;
					END IF;
					END IF;

			END IF;



		RETURN v_debit;
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Ass_sumDEBETalter1` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `BBranchMOVEMENT_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `BBranchMOVEMENT_GetNetTotal`(`p_AccName` BIGINT, `p_BranchID` INT, `p_ID` BIGINT, `p_CurrencyID` INT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
DECLARE v_IsDefault INT;
	SELECT IsDefault INTO v_IsDefault FROM CurrencyMainTb AS CMT
	where cmt.ID=p_CurrencyID
 ORDER BY CMT.ID DESC LIMIT 1;
	IF v_IsDefault=1
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID;
		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID;
		SELECT a.Credit INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID LIMIT 1;
		SELECT a.Debit INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID LIMIT 1;
SET v_AccVal=v_AccdebitVal-v_AccCreditVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;

	IF v_IsDefault=0
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;

		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;
		SELECT a.Credit INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
		SELECT a.Debit INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;


	RETURN v_NetVal;
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`BBranchMOVEMENT_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Branch_GetInteralBene`;
DELIMITER //
CREATE FUNCTION `Branch_GetInteralBene`(`p_RBranchID` INT, `p_DBranchID` INT, `p_InputVal` DECIMAL(12,3)) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_RBrType INT;
DECLARE v_DBrType INT;
DECLARE v_CulRes DECIMAL(12 ,3);
DECLARE v_RestShare DECIMAL(12 ,3);
DECLARE v_BrRt DECIMAL(12 ,3);
DECLARE v_RBShare DECIMAL(12 ,3);
DECLARE v_DBShare DECIMAL(12 ,3);
DECLARE v_DbrRt DECIMAL(12 ,3);
DECLARE v_AgentRestVal DECIMAL(12 ,3);
DECLARE v_BrVal DECIMAL(12 ,3);
DECLARE v_MainBr INT;
	SELECT ID INTO v_MainBr FROM   CoBranch
	WHERE  IsMain = 1 LIMIT 1;

	SET v_RBrType = ( SELECT BranchType FROM   CoBranch WHERE  ID = p_RBranchID );
	SET v_DBrType = ( SELECT BranchType FROM   CoBranch WHERE  ID = p_DBranchID );
		 IF p_RBranchID=v_MainBr
	     THEN
	        SELECT MainBranchBenefit INTO v_DbrRt FROM   CoBranch
	        WHERE  id = p_DBranchID;
SET v_DBShare = p_InputVal*v_DbrRt/100;
SET v_BrVal = p_InputVal- v_DBShare;
SET v_RestShare = 0.000;
	    END IF;
		 IF p_DBranchID=v_MainBr
	         THEN
	            SELECT MainBranchBenefit INTO v_BrRt FROM   CoBranch
	            WHERE  id = p_RBranchID;
SET v_RBShare = p_InputVal*v_BrRt/100;
SET v_BrVal = p_InputVal- v_RBShare;
SET v_RestShare = 0.000;
	        END IF;
			IF p_DBranchID<>v_MainBr and p_RBranchID <> v_MainBr and v_DBrType=1 AND v_RBrType=1
	     THEN


	            SELECT BranchRate INTO v_BrRt FROM   CoBranch
	            WHERE  id = p_RBranchID;

	            SELECT BranchRate INTO v_DbrRt FROM   CoBranch
	            WHERE  id = p_DBranchID LIMIT 1;
SET v_RBShare = p_InputVal*v_BrRt/100;
SET v_DBShare = p_InputVal*v_DbrRt/100;
SET v_CulRes = v_RBShare+v_DBShare;
	            IF v_CulRes<>0
	             THEN
SET v_RestShare = p_InputVal- v_CulRes;
	            END IF;

	END IF;

         RETURN IFNULL(v_BrVal, 0.000);
     END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Branch_GetInteralBene` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Currencyonthebank_forselecPruse`;
DELIMITER //
CREATE FUNCTION `Currencyonthebank_forselecPruse`(`p_Type` INT, `p_SalePricevalue` DECIMAL(12,3), `p_accFRom` INT, `p_ACCto` INT) RETURNS decimal(12,3)
    DETERMINISTIC
BEGIN
DECLARE v_SalePric DECIMAL(12, 3);

		SELECT CASE 								   WHEN p_Type = 1 									   THEN 									   p_SalePricevalue / a.SalePrice 								   ELSE 								   p_SalePricevalue / a.BuyPrice 						   END INTO v_SalePric FROM
				CurrencyPriceDetailsTb AS a
		WHERE
				a.CurrencyIDFrom = p_accFRom
				AND a.CurrencyIDTo = p_ACCto LIMIT 1;

		RETURN IFNULL(v_SalePric, 0);
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Currencyonthebank_forselecPruse` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `CUSTMOVEMENT_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `CUSTMOVEMENT_GetNetTotal`(`p_AccName` BIGINT, `p_BranchID` INT, `p_ID` BIGINT, `p_CurrencyID` INT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
DECLARE v_IsDefault INT;
	SELECT IsDefault INTO v_IsDefault FROM CurrencyMainTb AS CMT
	where cmt.ID=p_CurrencyID
 ORDER BY CMT.ID DESC LIMIT 1;
	IF v_IsDefault=1
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID;
		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID;
		SELECT a.Credit INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID LIMIT 1;
		SELECT a.Debit INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;

	IF v_IsDefault=0
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;

		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;
		SELECT a.Credit INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
		SELECT a.Debit INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;


	RETURN v_NetVal;
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`CUSTMOVEMENT_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `CUSTMOVEMENT_GetNetTotalBasidONBankORNot`;
DELIMITER //
CREATE FUNCTION `CUSTMOVEMENT_GetNetTotalBasidONBankORNot`(`p_AccName` BIGINT, `p_BranchID` INT, `p_ID` BIGINT, `p_CurrencyID` INT, `p_Type` TINYINT UNSIGNED) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
DECLARE v_IsDefault INT;
	SELECT IsDefault INTO v_IsDefault FROM CurrencyMainTb AS CMT
	where cmt.ID=p_CurrencyID
 ORDER BY CMT.ID DESC LIMIT 1;

	IF p_Type=0
	 THEN
	IF v_IsDefault=1
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID;
		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID;
		SELECT a.Credit INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID LIMIT 1;
		SELECT a.Debit INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;

	IF v_IsDefault=0
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;

		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;
		SELECT a.Credit INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
		SELECT a.Debit INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;
	END IF;


	IF p_Type=1
	 THEN
	IF v_IsDefault=1
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.ISBank=0;
		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.ISBank=0;
		SELECT a.Credit INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.ISBank=0 LIMIT 1;
		SELECT a.Debit INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.ISBank=0 LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;

	IF v_IsDefault=0
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;

		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;
		SELECT a.Credit INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
		SELECT a.Debit INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;
	END IF;




	IF p_Type=2
	 THEN
	IF v_IsDefault=1
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.ISBank=1;
		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.ISBank=1;
		SELECT a.Credit INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.ISBank=1 LIMIT 1;
		SELECT a.Debit INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.ISBank=1 LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;

	IF v_IsDefault=0
	 THEN
		SELECT IFNULL( SUM( a.Credit ), 0.000 ) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;

		SELECT IFNULL( SUM( a.Debit ), 0.000 ) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID<p_ID
			  AND a.CurrencyID=p_CurrencyID;
		SELECT a.Credit INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
		SELECT a.Debit INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
		WHERE a.IsActive=1
			  AND a.AccIDFrom=p_AccName
			  AND a.AccBranchID=p_BranchID
			  AND a.ID=p_ID
			  AND a.CurrencyID=p_CurrencyID LIMIT 1;
SET v_AccVal=v_AccCreditVal-v_AccdebitVal;
		IF v_CurrenCredit>v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal+v_CurrenCredit;
		END IF;

		IF v_CurrenCredit<v_CurrenDebit
		 THEN
SET v_NetVal=v_AccVal-v_CurrenDebit;
		END IF;

	END IF;
	END IF;

	RETURN v_NetVal;
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`CUSTMOVEMENT_GetNetTotalBasidONBankORNot` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `CustTB_GetIsHaideenAcc`;
DELIMITER //
CREATE FUNCTION `CustTB_GetIsHaideenAcc`(`p_AccName` BIGINT, `p_UserID` INT) RETURNS tinyint(1)
    DETERMINISTIC
BEGIN
DECLARE v_IsHaideen bit;
DECLARE v_CanShowHACC BIT;

SELECT IFNULL(HiddenAccount,0) INTO v_IsHaideen FROM AccountsTb a
INNER JOIN CustomersTb b ON a.AccID=b.AccID  WHERE a.AccID=p_AccName LIMIT 1;
SELECT tu.CanShowHACC INTO v_CanShowHACC FROM TB_Users tu WHERE tu.USID=p_UserID
 ORDER BY tu.USID DESC LIMIT 1;

IF v_IsHaideen=1 AND v_CanShowHACC=1
 THEN
SET v_IsHaideen=0;
END IF;




	RETURN IFNULL(v_IsHaideen,0);
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`CustTB_GetIsHaideenAcc` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `EMPMOVEMENT_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `EMPMOVEMENT_GetNetTotal`(`p_AccName` BIGINT, `p_BranchID` INT, `p_ID` BIGINT, `p_CurrencyTo` INT, `p_Type` TINYINT UNSIGNED) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
DECLARE v_CurrencyID BIT;
		SELECT IsDefault INTO v_CurrencyID FROM
				CurrencyMainTb AS CMT
		WHERE
				ID = p_CurrencyTo
 ORDER BY CMT.ID DESC LIMIT 1;



		IF v_CurrencyID = 1
			 THEN

			IF p_Type = 0
			 THEN
				SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13);
				SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccdebitVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13);
				SELECT a.Credit INTO v_CurrenCredit FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13) LIMIT 1;
				SELECT a.Debit INTO v_CurrenDebit FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13) LIMIT 1;
SET v_AccVal = v_AccCreditVal - v_AccdebitVal;
				IF v_CurrenCredit > v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal + v_CurrenCredit;
					END IF;

				IF v_CurrenCredit < v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal - v_CurrenDebit;
					END IF;

			END IF;
			END IF;


			IF p_Type = 1
			 THEN
				SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=0;
				SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccdebitVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=0;
				SELECT a.Credit INTO v_CurrenCredit FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=0 LIMIT 1;
				SELECT a.Debit INTO v_CurrenDebit FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=0 LIMIT 1;
SET v_AccVal = v_AccCreditVal - v_AccdebitVal;
				IF v_CurrenCredit > v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal + v_CurrenCredit;
					END IF;

				IF v_CurrenCredit < v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal - v_CurrenDebit;
					END IF;


			END IF;


						IF p_Type = 2
			 THEN
				SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=1;
				SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccdebitVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=1;
				SELECT a.Credit INTO v_CurrenCredit FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=1 LIMIT 1;
				SELECT a.Debit INTO v_CurrenDebit FROM
						ExSyAccounts_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND (a.CurrencyID = p_CurrencyTo or a.CurrencyID =13)
						AND a.IsBank=1 LIMIT 1;
SET v_AccVal = v_AccCreditVal - v_AccdebitVal;
				IF v_CurrenCredit > v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal + v_CurrenCredit;
					END IF;

				IF v_CurrenCredit < v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal - v_CurrenDebit;
					END IF;


			END IF;

		IF v_CurrencyID = 0
			 THEN

				SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccCreditVal FROM
						ExSyAccountsCurrency_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID;
				SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccdebitVal FROM
						ExSyAccountsCurrency_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID
						AND a.CurrencyID = p_CurrencyTo;
				SELECT a.Debit INTO v_CurrenCredit FROM
						ExSyAccountsCurrency_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND a.CurrencyID = p_CurrencyTo LIMIT 1;
				SELECT a.Credit INTO v_CurrenDebit FROM
						ExSyAccountsCurrency_AccSafeActivityTb AS a
					LEFT JOIN
						AccountsTb AS b
							ON a.AccIDFrom = b.AccID
				WHERE
						a.IsActive = 1
						AND a.AccIDFrom = p_AccName
						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID
						AND a.CurrencyID = p_CurrencyTo LIMIT 1;
SET v_AccVal = v_AccCreditVal - v_AccdebitVal;
				IF v_CurrenCredit > v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal + v_CurrenCredit;
					END IF;

				IF v_CurrenCredit < v_CurrenDebit
					 THEN
SET v_NetVal = v_AccVal - v_CurrenDebit;
					END IF;

			END IF;




		RETURN v_NetVal;
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`EMPMOVEMENT_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `EMP_GetNettoals`;
DELIMITER //
CREATE FUNCTION `EMP_GetNettoals`(`p_EMPID` INT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15 , 3);
DECLARE v_TempVal DECIMAL(15 , 3);
DECLARE v_ConsatanceVal DECIMAL(15 , 3);
DECLARE v_FinalTotal DECIMAL(15 , 3);


    SELECT b.SalaryVal + `EMP_GetTempIncreaseToals`(p_EMPID) + `EMP_GetConstanceIncreaseToals`(p_EMPID) - `EMP_GetDiscTtoals`(p_EMPID) - `EMP_GetADVPMNTTtoals`(p_EMPID) - EMP_GetLeaveDiscountTotal(p_EMPID) INTO v_AccCreditVal FROM EmployeeTb AS b
    WHERE b.ID = p_EMPID
          AND
          b.IsActive = 1 LIMIT 1;
    RETURN IFNULL(v_AccCreditVal , 0.000);
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`EMP_GetNettoals` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `EXET_GetSalePrice`;
DELIMITER //
CREATE FUNCTION `EXET_GetSalePrice`(`p_CurrencyTo` INT, `p_CurrencyFrom` INT, `p_AccountType` INT, `p_CountryID` INT, `p_BankID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_Get_Purchaseprice FLOAT;




        IF p_CurrencyTo = 1
           THEN
            SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
                    NewCurrencyPricesOwnTb AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND a.CurrencyIDFrom = p_CurrencyTo
                    AND b.CurrencyIDTo = p_CurrencyFrom
                    AND a.PriceType = 2
                    AND a.CountryID = p_CountryID
                    AND a.AccountType=p_AccountType;
          END IF;

        IF p_CurrencyTo <> 1
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = 2
                    AND b.CountryID = p_CountryID
                    AND b.AccountType=p_AccountType;

            IF v_CurrencyPower = 1
               THEN
                SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.CountryID = p_CountryID
                        AND b.AccountType=p_AccountType;
                SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.CountryID = p_CountryID
                        AND b.AccountType=p_AccountType LIMIT 1;
                SELECT CASE                                                     WHEN a.CurrencyPower = 0                                                       THEN                                                       v_CurrencySalePrice * v_CurrencyBuyPrice                                                     ELSE                                                     v_CurrencyBuyPrice / v_CurrencySalePrice                                             END INTO v_Get_Purchaseprice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.CountryID = p_CountryID
                        AND b.AccountType=p_AccountType LIMIT 1;
              END IF;

            IF v_CurrencyPower = 0
               THEN
                SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.CountryID = p_CountryID
                        AND b.AccountType=p_AccountType;
                SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.CountryID = p_CountryID
                        AND b.AccountType=p_AccountType LIMIT 1;
                SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.CountryID = p_CountryID
                        AND b.AccountType=p_AccountType LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
              END IF;

          END IF;



    RETURN IFNULL(v_Get_Purchaseprice, 0);
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`EXET_GetSalePrice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `EXEX_Get_TransBuyprice`;
DELIMITER //
CREATE FUNCTION `EXEX_Get_TransBuyprice`(`p_CurrencyFrom` INT, `p_CurrencyTo` INT, `p_AccountType` INT, `p_BranchID` INT, `p_CountryID` INT, `p_BankID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_DefaolfCurr INT;
DECLARE v_Get_Purchaseprice FLOAT;


    IF (p_AccountType = 0 OR p_AccountType = 3)
       THEN
        IF (p_CurrencyTo = 1
          OR
          p_CurrencyFrom = 1)
           THEN
            SELECT b.BuyPrice INTO v_Get_Purchaseprice FROM
                    `NewCurrencyPricesOwnTb` AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND (   b.CurrencyIDTo = p_CurrencyTo
                            OR b.CurrencyIDTo = p_CurrencyFrom)
                    AND a.PriceType = 2
                    AND a.AccountType = p_AccountType
					AND a.CountryID =p_CountryID
          AND BankID=p_BankID;
          END IF;
        IF p_CurrencyTo <> 1
          AND
          p_CurrencyFrom <> 1
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  `NewCurrencyPricesOwnTb` AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = 2
                    AND b.AccountType = p_AccountType
					AND b.CountryID =p_CountryID
          AND BankID=p_BankID;
            IF v_CurrencyPower = 1
               THEN
                SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT CASE                                                     WHEN a.CurrencyPower = 0                                                       THEN                                                       v_CurrencySalePrice * v_CurrencyBuyPrice                                                     ELSE                                                     v_CurrencyBuyPrice / v_CurrencySalePrice                                             END INTO v_Get_Purchaseprice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
              END IF;
            IF v_CurrencyPower = 0
               THEN
                SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
              END IF;
          END IF;
      END IF;
    IF p_AccountType = 1
       THEN
        IF (p_CurrencyTo = 1
          OR
          p_CurrencyFrom = 1)
           THEN
            SELECT b.BuyPrice INTO v_Get_Purchaseprice FROM
                    `NewCurrencyPricesOwnTb` AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND (   b.CurrencyIDTo = p_CurrencyTo
                            OR b.CurrencyIDTo = p_CurrencyFrom)
                    AND a.PriceType = 2
                    AND a.BranchID = p_BranchID
                    AND a.AccountType = p_AccountType
					AND a.CountryID =p_CountryID
          AND BankID=p_BankID;
          END IF;
        IF (p_CurrencyTo <> 1
          AND
          p_CurrencyFrom <> 1)
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  `NewCurrencyPricesOwnTb` AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = 2
                    AND b.BranchID = p_BranchID
                    AND b.AccountType = p_AccountType
					AND b.CountryID =p_CountryID
          AND BankID=p_BankID;
            IF v_CurrencyPower = 1
               THEN
                SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT CASE                                                     WHEN a.CurrencyPower = 0                                                       THEN                                                       v_CurrencySalePrice * v_CurrencyBuyPrice                                                     ELSE                                                     v_CurrencyBuyPrice / v_CurrencySalePrice                                             END INTO v_Get_Purchaseprice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
              END IF;
            IF v_CurrencyPower = 0
               THEN
                SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
              END IF;
          END IF;
      END IF;


    RETURN IFNULL(v_Get_Purchaseprice, 0);

  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`EXEX_Get_TransBuyprice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `EXEX_Get_Transprice`;
DELIMITER //
CREATE FUNCTION `EXEX_Get_Transprice`(`p_CurrencyFrom` INT, `p_CurrencyTo` INT, `p_AccountType` INT, `p_BranchID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_DefaolfCurr int;
DECLARE v_Get_Purchaseprice FLOAT;


				If p_AccountType=0
				 THEN
				IF p_CurrencyTo = 1 OR p_CurrencyFrom=1
					 THEN
						SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
								`NewCurrencyPricesOwnTb` AS a
							INNER JOIN
								NewCurrencyPriceOwnDetailsTb AS b
									ON a.ID = b.CPID
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
						WHERE
								a.IsActive = 1
								AND b.IsActive = 1
								AND c.IsActive = 1
								AND (b.CurrencyIDTo =  p_CurrencyTo or b.CurrencyIDTo =p_CurrencyFrom)
								AND a.PriceType=2;
					END IF;
				IF p_CurrencyTo <> 1 AND p_CurrencyFrom<>1
					 THEN
						SELECT a.CurrencyPower INTO v_CurrencyPower FROM
								NewCurrencyPriceOwnDetailsTb AS a
								INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyFrom
								AND b.PriceType=2;
						IF v_CurrencyPower = 1
							 THEN
								SELECT a.SalePrice INTO v_CurrencySalePrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo
										AND b.PriceType=2;

								SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom
										AND b.PriceType=2 LIMIT 1;
								SELECT CASE 																	WHEN a.CurrencyPower = 0 																		THEN 																		v_CurrencySalePrice * v_CurrencyBuyPrice 																	ELSE 																	v_CurrencyBuyPrice / v_CurrencySalePrice 															END INTO v_Get_Purchaseprice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDTo = p_CurrencyTo
										AND b.PriceType=2 LIMIT 1;
							END IF;
						IF v_CurrencyPower = 0
							 THEN
								SELECT a.CurrencyPower INTO v_CurrencyPower FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom
										AND b.PriceType=2;
								SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom
										AND b.PriceType=2 LIMIT 1;
								SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo
										AND b.PriceType=2 LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
							END IF;
					END IF;
					END IF;



		        If p_AccountType=1
				 THEN
				IF p_CurrencyTo = 1 AND p_CurrencyFrom=1
					 THEN
						SELECT b.BuyPrice INTO v_Get_Purchaseprice FROM
								`NewCurrencyPricesOwnTb` AS a
							INNER JOIN
								NewCurrencyPriceOwnDetailsTb AS b
									ON a.ID = b.CPID
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
						WHERE
								a.IsActive = 1
								AND b.IsActive = 1
								AND c.IsActive = 1
								AND (b.CurrencyIDTo =  p_CurrencyTo or b.CurrencyIDTo =p_CurrencyFrom)
								AND a.PriceType=2
								AND a.BranchID=p_BranchID;
					END IF;
				IF p_CurrencyTo <> 1 or p_CurrencyFrom<>1
					 THEN
						SELECT a.CurrencyPower INTO v_CurrencyPower FROM
								NewCurrencyPriceOwnDetailsTb AS a
								INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyFrom
								AND b.PriceType=2
								AND b.BranchID=p_BranchID;
						IF v_CurrencyPower = 1
							 THEN
								SELECT a.SalePrice INTO v_CurrencySalePrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo
										AND b.PriceType=2
										AND b.BranchID=p_BranchID;
								SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom
										AND b.PriceType=2
										AND b.BranchID=p_BranchID LIMIT 1;
								SELECT CASE 																	WHEN a.CurrencyPower = 0 																		THEN 																		v_CurrencySalePrice * v_CurrencyBuyPrice 																	ELSE 																	v_CurrencyBuyPrice / v_CurrencySalePrice 															END INTO v_Get_Purchaseprice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDTo = p_CurrencyTo
										AND b.PriceType=2
										AND b.BranchID=p_BranchID LIMIT 1;
							END IF;
						IF v_CurrencyPower = 0
							 THEN
								SELECT a.CurrencyPower INTO v_CurrencyPower FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom
										AND b.PriceType=2;
								SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom
										AND b.PriceType=2
										AND b.BranchID=p_BranchID LIMIT 1;
								SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
										NewCurrencyPriceOwnDetailsTb AS a
										INNER JOIN
								`NewCurrencyPricesOwnTb` AS b
								ON a.CPID = b.ID
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo
										AND b.PriceType=2
										AND b.BranchID=p_BranchID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
							END IF;
					END IF;
					END IF;

		RETURN IFNULL(v_Get_Purchaseprice, 0);

	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`EXEX_Get_Transprice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `EXEX_Get_TransSaleprice`;
DELIMITER //
CREATE FUNCTION `EXEX_Get_TransSaleprice`(`p_CurrencyFrom` INT, `p_CurrencyTo` INT, `p_AccountType` INT, `p_BranchID` INT, `p_CountryID` INT, `p_BankID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_DefaolfCurr INT;
DECLARE v_Get_Purchaseprice FLOAT;


    IF (p_AccountType = 0
      OR
      p_AccountType = 3)
       THEN
        IF (p_CurrencyTo = 1
          OR
          p_CurrencyFrom = 1)
           THEN
            SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
                    `NewCurrencyPricesOwnTb` AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND (   b.CurrencyIDTo = p_CurrencyTo
                            OR b.CurrencyIDTo = p_CurrencyFrom)
                    AND a.PriceType = 2
                    AND a.AccountType = p_AccountType
					AND a.CountryID =p_CountryID
          AND BankID=p_BankID;
          END IF;
        IF p_CurrencyTo <> 1
          AND
          p_CurrencyFrom <> 1
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  `NewCurrencyPricesOwnTb` AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = 2
                    AND b.AccountType = p_AccountType
					AND b.CountryID =p_CountryID
          AND BankID=p_BankID;
            IF v_CurrencyPower = 1
               THEN
                SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT CASE                                                     WHEN a.CurrencyPower = 0                                                       THEN                                                       v_CurrencySalePrice * v_CurrencyBuyPrice                                                     ELSE                                                     v_CurrencyBuyPrice / v_CurrencySalePrice                                             END INTO v_Get_Purchaseprice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID LIMIT 1;
              END IF;
            IF v_CurrencyPower = 0
               THEN
                SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
              END IF;
          END IF;
      END IF;



    IF p_AccountType = 1
       THEN
        IF (p_CurrencyTo = 1
          OR
          p_CurrencyFrom = 1)
           THEN
            SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
                    `NewCurrencyPricesOwnTb` AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND (   b.CurrencyIDTo = p_CurrencyTo
                            OR b.CurrencyIDTo = p_CurrencyFrom)
                    AND a.PriceType = 2
                    AND a.BranchID = p_BranchID
                    AND a.AccountType = p_AccountType
					AND a.CountryID =p_CountryID
          AND BankID=p_BankID;
          END IF;
        IF (p_CurrencyTo <> 1
          AND
          p_CurrencyFrom <> 1)
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  `NewCurrencyPricesOwnTb` AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = 2
                    AND b.BranchID = p_BranchID
                    AND b.AccountType = p_AccountType
					AND b.CountryID =p_CountryID
          AND BankID=p_BankID;
            IF v_CurrencyPower = 1
               THEN
                SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT CASE                                                     WHEN a.CurrencyPower = 0                                                       THEN                                                       v_CurrencySalePrice * v_CurrencyBuyPrice                                                     ELSE                                                     v_CurrencyBuyPrice / v_CurrencySalePrice                                             END INTO v_Get_Purchaseprice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
              END IF;
            IF v_CurrencyPower = 0
               THEN
                SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID;
                SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
                SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      `NewCurrencyPricesOwnTb` AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = 2
                        AND b.BranchID = p_BranchID
                        AND b.AccountType = p_AccountType
						AND b.CountryID =p_CountryID
            AND BankID=p_BankID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
              END IF;
          END IF;
      END IF;

    RETURN IFNULL(v_Get_Purchaseprice, 0);

  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`EXEX_Get_TransSaleprice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `GetAccline`;
DELIMITER //
CREATE FUNCTION `GetAccline`(`p_Parent` BIGINT) RETURNS int(11)
    DETERMINISTIC
BEGIN
DECLARE v_Accline int;
SELECT Accline INTO v_Accline FROM AccountsTb WHERE AccCode  = p_Parent LIMIT 1;
IF v_Accline >= 0
 THEN
 SET v_Accline = v_Accline + (1);
 END IF;
IF v_Accline IS  NULL
 THEN
SET v_Accline =0;
END IF;


 RETURN  v_Accline;
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`GetAccline` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `GETSafeIDMovement`;
DELIMITER //
CREATE FUNCTION `GETSafeIDMovement`(`p_FirstCurrency` INT, `p_SAFTypeform` INT, `p_ISbaunk` INT, `p_SafTypeTo` INT, `p_SAFFACCTO` INT, `p_UeserInset` INT, `p_SaFACCount` INT, `p_typeFRom` INT, `p_BPrice1` DOUBLE, `p_Purchaseprice` DECIMAL(13,3)) RETURNS longtext CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci
    DETERMINISTIC
BEGIN
DECLARE v_GETSafeIDMovement LONGTEXT;
DECLARE v_CRubsename LONGTEXT;
DECLARE v_SAFTypefo LONGTEXT;
DECLARE v_SAFFACC LONGTEXT;
DECLARE v_UeserI			   LONGTEXT;


		SELECT AccName INTO v_SAFFACC FROM
				AccountsTb
		WHERE
				AccID = p_SAFFACCTO LIMIT 1;

		SELECT a.CurCode INTO v_CRubsename FROM
				CurrencyMainTb AS a
		WHERE
				a.ID = p_FirstCurrency
 ORDER BY a.ID DESC LIMIT 1;
		SELECT AccName INTO v_SAFTypefo FROM
				AccountsTb
		WHERE
				AccID = p_SaFACCount LIMIT 1;
		SELECT UName INTO v_UeserI FROM
				TB_Users
		WHERE
				USID = p_UeserInset
 ORDER BY TB_Users.USID DESC LIMIT 1;
		IF p_ISbaunk = 0
			 THEN
SET v_GETSafeIDMovement = CONCAT(CASE 													WHEN p_typeFRom = 0 														THEN 														'????' 													ELSE 													'???' 											END, SPACE(1), CAST(p_BPrice1 AS CHAR), SPACE(1), v_CRubsename, CHAR(10), CHAR(13), SPACE(1), '???? ', CAST(p_Purchaseprice AS CHAR), SPACE(1), CASE 										  WHEN p_typeFRom = 0 											  THEN 											  CASE 													  WHEN p_SAFTypeform = 0 														  THEN 														  '?????' 													  ELSE 													  CONCAT(CASE 															  WHEN p_SAFTypeform = 2151 																  THEN 																  '?? ???? ?????? ' 															  ELSE 															  '?? ' 													  END, '  ', v_SAFTypefo) 											  END 										  ELSE 										  '' 								  END, CHAR(13), CHAR(10), CASE 													 WHEN p_typeFRom = 0 														 THEN 														 CASE 																 WHEN p_SafTypeTo <> 0 																	 THEN CONCAT(SPACE(1), ' ?????') 																 ELSE 																 SPACE(1) 														 END 													 ELSE 													 CASE 															 WHEN p_SafTypeTo <> 0 																 THEN 																 CONCAT(SPACE(1), ' ?????') 															 ELSE 															 '???? ' 													 END  											 END);
			END IF;
		RETURN IFNULL(v_GETSafeIDMovement, '???? ???? ?????? ???????');

	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`GETSafeIDMovement` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Get_Purchaseprice`;
DELIMITER //
CREATE FUNCTION `Get_Purchaseprice`(`p_CurrencyTo` INT, `p_CurrencyFrom` INT, `p_TupeBank` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_Get_Purchaseprice FLOAT;

		IF p_TupeBank = 0
			 THEN
				IF p_CurrencyTo = 1
					 THEN
						SELECT b.BuyPrice INTO v_Get_Purchaseprice FROM
								`CurrencyPricesTb` AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
						WHERE
								a.IsActive = 1
								AND b.IsActive = 1
								AND c.IsActive = 1
								AND a.CurrencyIDFrom = p_CurrencyTo
								AND b.CurrencyIDTo = p_CurrencyFrom;
					END IF;
				IF p_CurrencyTo <> 1
					 THEN
						SELECT a.CurrencyPower INTO v_CurrencyPower FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyFrom;
						IF v_CurrencyPower = 1
							 THEN
								SELECT a.SalePrice INTO v_CurrencySalePrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo;

								SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom LIMIT 1;
								SELECT CASE 																	WHEN a.CurrencyPower = 0 																		THEN 																		v_CurrencySalePrice * v_CurrencyBuyPrice 																	ELSE 																	v_CurrencyBuyPrice / v_CurrencySalePrice 															END INTO v_Get_Purchaseprice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDTo = p_CurrencyTo LIMIT 1;
							END IF;
						IF v_CurrencyPower = 0
							 THEN
								SELECT a.CurrencyPower INTO v_CurrencyPower FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom;
								SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom LIMIT 1;
								SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
							END IF;
					END IF;



			END IF;
		RETURN IFNULL(v_Get_Purchaseprice, 0);

	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Get_Purchaseprice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Get_PurchaseSalePrice`;
DELIMITER //
CREATE FUNCTION `Get_PurchaseSalePrice`(`p_CurrencyTo` INT, `p_CurrencyFrom` INT, `p_TupeBank` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_Get_Purchaseprice FLOAT;

		IF p_TupeBank = 0
			 THEN

				IF p_CurrencyTo = 1
					 THEN
						SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
								`CurrencyPricesTb` AS a
							LEFT JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							LEFT JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
						WHERE
								a.IsActive = 1
								AND b.IsActive = 1
								AND c.IsActive = 1
								AND a.CurrencyIDFrom = p_CurrencyTo
								AND b.CurrencyIDTo = p_CurrencyFrom;
					END IF;

				IF p_CurrencyTo <> 1
					 THEN
						SELECT a.CurrencyPower INTO v_CurrencyPower FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyFrom;

						IF v_CurrencyPower = 1
							 THEN
								SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo;
								SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom LIMIT 1;
								SELECT CASE 																	WHEN a.CurrencyPower = 0 																		THEN 																		v_CurrencySalePrice * v_CurrencyBuyPrice 																	ELSE 																	v_CurrencyBuyPrice / v_CurrencySalePrice 															END INTO v_Get_Purchaseprice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDTo = p_CurrencyTo LIMIT 1;
							END IF;

						IF v_CurrencyPower = 0
							 THEN
								SELECT a.CurrencyPower INTO v_CurrencyPower FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom;
								SELECT a.SalePrice INTO v_CurrencySalePrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyFrom LIMIT 1;
								SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
										CurrencyPriceDetailsTb AS a
								WHERE
										a.CurrencyIDFrom = 1
										AND a.CurrencyIDTo = p_CurrencyTo LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
							END IF;

					END IF;

			END IF;

		RETURN IFNULL(v_Get_Purchaseprice, 0);
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Get_PurchaseSalePrice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `GET_ReturnsSafeIDMovement`;
DELIMITER //
CREATE FUNCTION `GET_ReturnsSafeIDMovement`(`p_FirstCurrency` INT, `p_SAFTypeform` INT, `p_ISbaunk` INT, `p_SafTypeTo` INT, `p_SAFFACCTO` INT, `p_UeserInset` INT, `p_SaFACCount` INT, `p_typeFRom` INT, `p_BPrice1` DOUBLE, `p_Purchaseprice` DECIMAL(13,3)) RETURNS longtext CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci
    DETERMINISTIC
BEGIN
DECLARE v_GETSafeIDMovement LONGTEXT;
DECLARE v_CRubsename LONGTEXT;
DECLARE v_SAFTypefo LONGTEXT;
DECLARE v_SAFFACC LONGTEXT;
DECLARE v_UeserI            LONGTEXT;


  SELECT AccName INTO v_SAFFACC FROM AccountsTb
  WHERE AccID=p_SAFFACCTO LIMIT 1;
  SELECT a.CurCode INTO v_CRubsename FROM CurrencyMainTb AS a
  WHERE a.ID=p_FirstCurrency
 ORDER BY a.ID DESC LIMIT 1;
  SELECT AccName INTO v_SAFTypefo FROM AccountsTb
  WHERE AccID=p_SaFACCount LIMIT 1;
  SELECT UName INTO v_UeserI FROM TB_Users
  WHERE USID=p_UeserInset
 ORDER BY TB_Users.USID DESC LIMIT 1;

  IF p_ISbaunk=0
   THEN
SET v_GETSafeIDMovement=CONCAT('????? ???????', SPACE( 1 ), CASE              WHEN p_typeFRom=0              THEN '????'              ELSE '???'     END, SPACE( 1 ), CAST(p_BPrice1 AS CHAR), SPACE( 1 ), v_CRubsename, CHAR( 10 ), CHAR( 13 ), SPACE( 1 ), '???? ', CAST(p_Purchaseprice AS CHAR), SPACE( 1 ), CASE              WHEN p_typeFRom=0              THEN CASE                       WHEN p_SAFTypeform=0                       THEN '?????'                       ELSE CONCAT(CASE                                WHEN p_SAFTypeform=2151                                THEN '?? ???? ?????? '                                ELSE '?? '                       END, '  ', v_SAFTypefo)              END              ELSE ''     END, CHAR( 13 ), CHAR( 10 ), CASE              WHEN p_typeFRom=0              THEN CASE                       WHEN p_SafTypeTo<>0                       THEN CONCAT(SPACE( 1 ), '')                       ELSE SPACE( 1 )              END              ELSE CASE                       WHEN p_SafTypeTo<>0                       THEN CONCAT(SPACE( 1 ), ' ')                       ELSE '???? '              END      END);
  END IF;

  RETURN IFNULL( v_GETSafeIDMovement, '???? ???? ?????? ???????' );
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`GET_ReturnsSafeIDMovement` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `NewGet_PurchaseSalePrice`;
DELIMITER //
CREATE FUNCTION `NewGet_PurchaseSalePrice`(`p_CurrencyTo` INT, `p_CurrencyFrom` INT, `p_TupeBank` INT, `p_CountryID` INT, `p_BankID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_Get_Purchaseprice FLOAT;

    IF (p_TupeBank = 0
      OR
      p_TupeBank = 1)
       THEN


        IF p_CurrencyTo = 1
           THEN
            SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
                    NewCurrencyPricesOwnTb AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND a.CurrencyIDFrom = p_CurrencyTo
                    AND b.CurrencyIDTo = p_CurrencyFrom
                    AND a.PriceType = p_TupeBank
                    AND a.CountryID = p_CountryID;
          END IF;

        IF p_CurrencyTo <> 1
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID;

            IF v_CurrencyPower = 1
               THEN
                SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = p_TupeBank
                        AND b.CountryID = p_CountryID;
                SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = p_TupeBank
                        AND b.CountryID = p_CountryID LIMIT 1;
                SELECT CASE                                                     WHEN a.CurrencyPower = 0                                                       THEN                                                       v_CurrencySalePrice * v_CurrencyBuyPrice                                                     ELSE                                                     v_CurrencyBuyPrice / v_CurrencySalePrice                                             END INTO v_Get_Purchaseprice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = p_TupeBank
                        AND b.CountryID = p_CountryID LIMIT 1;
              END IF;

            IF v_CurrencyPower = 0
               THEN
                SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = p_TupeBank
                        AND b.CountryID = p_CountryID;
                SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyFrom
                        AND b.PriceType = p_TupeBank
                        AND b.CountryID = p_CountryID LIMIT 1;
                SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                        NewCurrencyPriceOwnDetailsTb AS a
                    INNER JOIN
                      NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                WHERE
                        a.CurrencyIDFrom = 1
                        AND a.CurrencyIDTo = p_CurrencyTo
                        AND b.PriceType = p_TupeBank
                        AND b.CountryID = p_CountryID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
              END IF;

          END IF;

      END IF;

    IF p_TupeBank = 3
       THEN
        IF p_CurrencyTo = 1
           THEN
            SELECT b.SalePrice INTO v_Get_Purchaseprice FROM
                    NewCurrencyPricesOwnTb AS a
                INNER JOIN
                  NewCurrencyPriceOwnDetailsTb AS b
                    ON a.ID = b.CPID
                INNER JOIN
                  `CurrencyMainTb` AS c
                    ON b.CurrencyIDTo = c.ID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND c.IsActive = 1
                    AND a.CurrencyIDFrom = p_CurrencyTo
                    AND b.CurrencyIDTo = p_CurrencyFrom
                    AND a.PriceType = p_TupeBank
                    AND a.CountryID = p_CountryID
                    AND a.BankID = p_BankID;
          END IF;

        IF p_CurrencyTo <> 1
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID;
          END IF;
        IF v_CurrencyPower = 1
           THEN
            SELECT a.BuyPrice INTO v_CurrencySalePrice FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyTo
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID;
            SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID LIMIT 1;
            SELECT CASE                                                 WHEN a.CurrencyPower = 0                                                   THEN                                                   v_CurrencySalePrice * v_CurrencyBuyPrice                                                 ELSE                                                 v_CurrencyBuyPrice / v_CurrencySalePrice                                         END INTO v_Get_Purchaseprice FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDTo = p_CurrencyTo
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID LIMIT 1;
          END IF;

        IF v_CurrencyPower = 0
           THEN
            SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID;
            SELECT a.SalePrice INTO v_CurrencySalePrice FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyFrom
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID LIMIT 1;
            SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM
                    NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN
                  NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            WHERE
                    a.CurrencyIDFrom = 1
                    AND a.CurrencyIDTo = p_CurrencyTo
                    AND b.PriceType = p_TupeBank
                    AND b.CountryID = p_CountryID
                    AND b.BankID = p_BankID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
          END IF;
      END IF;
    RETURN IFNULL(v_Get_Purchaseprice, 0);
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`NewGet_PurchaseSalePrice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `New_Get_Purchaseprice`;
DELIMITER //
CREATE FUNCTION `New_Get_Purchaseprice`(`p_CurrencyTo` INT, `p_CurrencyFrom` INT, `p_TupeBank` INT, `p_CountryID` INT, `p_BankID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_CurrencyPower INT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_Get_Purchaseprice FLOAT;

  IF (p_TupeBank = 0
    OR p_TupeBank = 1)
   THEN
    IF p_CurrencyTo = 1
     THEN
      SELECT b.BuyPrice INTO v_Get_Purchaseprice FROM NewCurrencyPricesOwnTb AS a
      INNER JOIN NewCurrencyPriceOwnDetailsTb AS b
        ON a.ID = b.CPID
      INNER JOIN `CurrencyMainTb` AS c
        ON b.CurrencyIDTo = c.ID
      WHERE a.IsActive = 1
      AND b.IsActive = 1
      AND c.IsActive = 1
      AND a.CurrencyIDFrom = p_CurrencyTo
      AND b.CurrencyIDTo = p_CurrencyFrom
      AND a.PriceType = p_TupeBank
      AND a.CountryID = p_CountryID
      and a.BankID=0;
    END IF;
    IF p_CurrencyTo <> 1
     THEN
      SELECT a.CurrencyPower INTO v_CurrencyPower FROM NewCurrencyPriceOwnDetailsTb AS a
      INNER JOIN NewCurrencyPricesOwnTb AS b
        ON a.CPID = b.ID
      WHERE a.CurrencyIDFrom = 1
      AND a.CurrencyIDTo = p_CurrencyFrom
      AND b.PriceType = p_TupeBank
      AND b.CountryID = p_CountryID
      and b.BankID=0;
      IF v_CurrencyPower = 1
       THEN
        SELECT a.SalePrice INTO v_CurrencySalePrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyTo
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=0;

        SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyFrom
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=0 LIMIT 1;
        SELECT CASE             WHEN a.CurrencyPower = 0 THEN v_CurrencySalePrice * v_CurrencyBuyPrice             ELSE v_CurrencyBuyPrice / v_CurrencySalePrice           END INTO v_Get_Purchaseprice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDTo = p_CurrencyTo
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=0 LIMIT 1;
      END IF;
      IF v_CurrencyPower = 0
       THEN
        SELECT a.CurrencyPower INTO v_CurrencyPower FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyFrom
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=0;
        SELECT a.BuyPrice INTO v_CurrencySalePrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyFrom
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=0 LIMIT 1;
        SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyTo
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=0 LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
      END IF;
    END IF;
  END IF;
   IF p_TupeBank = 3
   THEN
    IF p_CurrencyTo = 1
     THEN
      SELECT b.BuyPrice INTO v_Get_Purchaseprice FROM NewCurrencyPricesOwnTb AS a
      INNER JOIN NewCurrencyPriceOwnDetailsTb AS b
        ON a.ID = b.CPID
      INNER JOIN `CurrencyMainTb` AS c
        ON b.CurrencyIDTo = c.ID
      WHERE a.IsActive = 1
      AND b.IsActive = 1
      AND c.IsActive = 1
      AND a.CurrencyIDFrom = p_CurrencyTo
      AND b.CurrencyIDTo = p_CurrencyFrom
      AND a.PriceType = p_TupeBank
      AND a.CountryID = p_CountryID
      and a.BankID=p_BankID;
    END IF;
    IF p_CurrencyTo <> 1
     THEN
      SELECT a.CurrencyPower INTO v_CurrencyPower FROM NewCurrencyPriceOwnDetailsTb AS a
      INNER JOIN NewCurrencyPricesOwnTb AS b
        ON a.CPID = b.ID
      WHERE a.CurrencyIDFrom = 1
      AND a.CurrencyIDTo = p_CurrencyFrom
      AND b.PriceType = p_TupeBank
      AND b.CountryID = p_CountryID
      and b.BankID=p_BankID;
      IF v_CurrencyPower = 1
       THEN
        SELECT a.SalePrice INTO v_CurrencySalePrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyTo
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=p_BankID;

        SELECT a.BuyPrice INTO v_CurrencyBuyPrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyFrom
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=p_BankID LIMIT 1;
        SELECT CASE             WHEN a.CurrencyPower = 0 THEN v_CurrencySalePrice * v_CurrencyBuyPrice             ELSE v_CurrencyBuyPrice / v_CurrencySalePrice           END INTO v_Get_Purchaseprice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDTo = p_CurrencyTo
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=p_BankID LIMIT 1;
      END IF;
      IF v_CurrencyPower = 0
       THEN
        SELECT a.CurrencyPower INTO v_CurrencyPower FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyFrom
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=p_BankID;
        SELECT a.BuyPrice INTO v_CurrencySalePrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyFrom
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=p_BankID LIMIT 1;
        SELECT a.SalePrice INTO v_CurrencyBuyPrice FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b
          ON a.CPID = b.ID
        WHERE a.CurrencyIDFrom = 1
        AND a.CurrencyIDTo = p_CurrencyTo
        AND b.PriceType = p_TupeBank
        AND b.CountryID = p_CountryID
        and b.BankID=p_BankID LIMIT 1;
SET v_Get_Purchaseprice = v_CurrencySalePrice * v_CurrencyBuyPrice;
      END IF;
    END IF;
  END IF;
  RETURN IFNULL(v_Get_Purchaseprice, 0);

END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`New_Get_Purchaseprice` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `Rollback_Branch_Trinsfrim_me`;
DELIMITER //
CREATE FUNCTION `Rollback_Branch_Trinsfrim_me`(`p_CurrentAccID` INT, `p_RBrType` INT, `p_RecievedCurrencyID` INT, `p_NetTotla` DECIMAL(18,3), `p_BranchRecievedID` INT) RETURNS tinyint(1)
    DETERMINISTIC
BEGIN
DECLARE v_Rollback bit;
DECLARE v_CanDepit bit;
DECLARE v_IsLimted bit;
DECLARE v_LimtedVal decimal(18,3);
DECLARE v_TotalAllowedLimit decimal(18,3);

                        SELECT at.CanDebit, at.IsLimited, at.LimitedVal INTO v_CanDepit, v_IsLimted, v_LimtedVal FROM
                                AccountsTb at
                        WHERE
                                at.AccID = p_CurrentAccID LIMIT 1;

                         IF p_RBrType=3
  THEN
                        IF IFNULL(Account_GetAccVal(p_CurrentAccID, p_RecievedCurrencyID, 0), 0) < p_NetTotla
                          AND
                          v_CanDepit = 0
                           THEN
                         set v_Rollback = 0;
                          END IF;

                        IF
                        v_CanDepit = 1
                          AND
                          v_IsLimted = 1
                          AND
                          (v_LimtedVal + IFNULL(Account_GetAccVal(p_CurrentAccID, p_RecievedCurrencyID, 0), 0)) < p_NetTotla
                           THEN
                             set v_Rollback = 0;
                          END IF;
                      END IF;

IF p_RBrType <> 3
 THEN


    SELECT a.IsLimited, IFNULL(a.LimitedVal, 0) INTO v_IsLimted, v_LimtedVal FROM
        AccountsTb a
    WHERE
        a.AccID = p_CurrentAccID;


    IF v_IsLimted = 1
     THEN

        SET v_TotalAllowedLimit = IFNULL(v_LimtedVal + Branch_GetAccVal(p_BranchRecievedID), 0);
        IF p_NetTotla > v_TotalAllowedLimit
         THEN
          set v_Rollback = 0;
        END IF;
    END IF;


                      END IF;
return IFNULL(v_Rollback,1);

                      END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`Rollback_Branch_Trinsfrim_me` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SafeIDBankServices_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `SafeIDBankServices_GetNetTotal`(`p_AccName` BIGINT, `p_BankService` INT, `p_CurrencyID` INT, `p_ID` BIGINT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
    IF p_CurrencyID = 1
      OR
      p_CurrencyID = 13
       THEN
        SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM
                ExSyAccounts_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
            LEFT JOIN
              InternalEx ie
                ON a.ISID = ie.Code
                  AND c.ID = ie.ServiceType
            LEFT JOIN
              ExternalEx ee
                ON a.ISID = ee.Code
                  AND c.ID = ee.BankServiceType
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = p_CurrencyID
                        OR a.CurrencyID = 13)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND (   ie.ServiceType = p_BankService
                        OR ee.BankServiceType = p_BankService)
                AND a.id < p_ID;
        SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccdebitVal FROM
                ExSyAccounts_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
            LEFT JOIN
              InternalEx ie
                ON a.ISID = ie.Code
                  AND c.ID = ie.ServiceType
            LEFT JOIN
              ExternalEx ee
                ON a.ISID = ee.Code
                  AND c.ID = ee.BankServiceType
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = p_CurrencyID
                        OR a.CurrencyID = 13)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND (   ie.ServiceType = p_BankService
                        OR ee.BankServiceType = p_BankService)
                AND a.id < p_ID;


        SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenCredit FROM
                ExSyAccounts_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
            LEFT JOIN
              InternalEx ie
                ON a.ISID = ie.Code
                  AND c.ID = ie.ServiceType
            LEFT JOIN
              ExternalEx ee
                ON a.ISID = ee.Code
                  AND c.ID = ee.BankServiceType
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = p_CurrencyID
                        OR a.CurrencyID = 13)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND (   ie.ServiceType = p_BankService
                        OR ee.BankServiceType = p_BankService)
                AND a.id = p_ID LIMIT 1;
        SELECT IFNULL(a.Debit, 0.000) INTO v_CurrenDebit FROM
                ExSyAccounts_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
            LEFT JOIN
              InternalEx ie
                ON a.ISID = ie.Code
                  AND c.ID = ie.ServiceType
            LEFT JOIN
              ExternalEx ee
                ON a.ISID = ee.Code
                  AND c.ID = ee.BankServiceType
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = p_CurrencyID
                        OR a.CurrencyID = 13)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND (   ie.ServiceType = p_BankService
                        OR ee.BankServiceType = p_BankService)
                AND a.id = p_ID LIMIT 1;
SET v_AccVal = v_AccCreditVal - v_AccdebitVal;
        IF v_CurrenCredit > v_CurrenDebit
           THEN
SET v_NetVal = v_CurrenCredit + IFNULL(v_AccVal, 0.000);
          END IF;
        IF v_CurrenCredit < v_CurrenDebit
           THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) - v_CurrenDebit;
          END IF;
      END IF;




    IF p_CurrencyID <> 1
       THEN
        SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccCreditVal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = p_CurrencyID)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND a.id < p_ID;
        SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccdebitVal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = p_CurrencyID)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND a.id < p_ID;


        SELECT IFNULL(a.Debit, 0.000) INTO v_CurrenCredit FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = p_CurrencyID)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND a.id = p_ID LIMIT 1;
        SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenDebit FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
            JOIN
              BBranchTb AS b
                ON a.AccIDFrom = b.AccID
            JOIN
              bankServicesTB AS c
                ON c.BBranchID = b.id
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = p_CurrencyID)
                AND a.SafeID = p_AccName
                AND c.ID = p_BankService
                AND a.id = p_ID LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
        IF v_CurrenCredit > v_CurrenDebit
           THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) - v_CurrenCredit;
          END IF;
        IF v_CurrenCredit < v_CurrenDebit
           THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) + v_CurrenDebit;
          END IF;
      END IF;




















    RETURN v_NetVal;
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SafeIDBankServices_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SafeIDBank_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `SafeIDBank_GetNetTotal`(`p_AccName` BIGINT, `p_BranchID` INT, `p_CurrencyID` INT, `p_ID` BIGINT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID bigint;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
		 if p_CurrencyID = 1 or p_CurrencyID = 13
		  THEN
		 SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND (a.TypeID between 15 and 18)
		   AND a.id<p_ID;
         SELECT IFNULL(SUM(a.debit), 0.000) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND (a.TypeID between 15 and 18)
		   AND a.id<p_ID;


		   SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND (a.TypeID between 15 and 18)
		   AND a.id=p_ID LIMIT 1;
         SELECT IFNULL(a.debit, 0.000) INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND (a.TypeID between 15 and 18)
		   AND a.id=p_ID LIMIT 1;
SET v_AccVal =  v_AccCreditVal-v_AccdebitVal;
		   IF v_CurrenCredit > v_CurrenDebit
		    THEN
SET v_NetVal =v_CurrenCredit+IFNULL(v_AccVal,0.000);
		   END IF;
		   IF v_CurrenCredit < v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) - v_CurrenDebit;
		   END IF;
		   END IF;




		    if p_CurrencyID <> 1
		  THEN
		 SELECT IFNULL(SUM(a.debit), 0.000) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID )
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID;
         SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID )
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID;


		   SELECT IFNULL(a.debit, 0.000) INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID)
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID LIMIT 1;
         SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID)
           AND a.SafeID = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
		   IF v_CurrenCredit > v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) - v_CurrenCredit;
		   END IF;
		   IF v_CurrenCredit < v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) + v_CurrenDebit;
		   END IF;
		   END IF;




















         RETURN v_NetVal;
     END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SafeIDBank_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SafeIDCur_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `SafeIDCur_GetNetTotal`(`p_BranchID` INT, `p_CurrencyID` INT, `p_ID` BIGINT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID BIGINT;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
		IF p_CurrencyID = 1
			OR
			p_CurrencyID = 13
			 THEN
				SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
				WHERE
						a.IsActive = 1
						AND (   a.CurrencyID = p_CurrencyID
								OR a.CurrencyID = 13)

						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID;
				SELECT IFNULL(SUM(a.Debit), 0.000) INTO v_AccdebitVal FROM
						ExSyAccounts_AccSafeActivityTb AS a
				WHERE
						a.IsActive = 1
						AND (   a.CurrencyID = p_CurrencyID
								OR a.CurrencyID = 13)

						AND a.AccBranchID = p_BranchID
						AND a.ID < p_ID;


				SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenCredit FROM
						ExSyAccounts_AccSafeActivityTb AS a
				WHERE
						a.IsActive = 1
						AND (   a.CurrencyID = p_CurrencyID
								OR a.CurrencyID = 13)

						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID LIMIT 1;
				SELECT IFNULL(a.Debit, 0.000) INTO v_CurrenDebit FROM
						ExSyAccounts_AccSafeActivityTb AS a
				WHERE
						a.IsActive = 1
						AND (   a.CurrencyID = p_CurrencyID
								OR a.CurrencyID = 13)

						AND a.AccBranchID = p_BranchID
						AND a.ID = p_ID LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
				IF v_CurrenCredit > v_CurrenDebit
					 THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) - v_CurrenCredit;
					END IF;
				IF v_CurrenCredit < v_CurrenDebit
					 THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) + v_CurrenDebit;
					END IF;
			END IF;




		IF p_CurrencyID <> 1
			 THEN
				SELECT IFNULL(SUM(a.ACC_DEPET_TO), 0.000) INTO v_AccCreditVal FROM
						ExSyAccountsCurrency_CurrencymovementPruse AS a
				WHERE
						a.IsActive = 1
						AND (a.AccFrom = p_CurrencyID)

						AND a.BID = p_BranchID
						AND a.ID < p_ID;
				SELECT IFNULL(SUM(a.CRedetTO), 0.000) INTO v_AccdebitVal FROM
						ExSyAccountsCurrency_CurrencymovementPruse AS a
				WHERE
						a.IsActive = 1
						AND (a.AccFrom = p_CurrencyID)

						AND a.BID = p_BranchID
						AND a.ID < p_ID;


				SELECT IFNULL(a.ACC_DEPET_TO, 0.000) INTO v_CurrenCredit FROM
						ExSyAccountsCurrency_CurrencymovementPruse AS a
				WHERE
						a.IsActive = 1
						AND (a.AccFrom = p_CurrencyID)

						AND a.BID = p_BranchID
						AND a.ID = p_ID LIMIT 1;
				SELECT IFNULL(a.CRedetTO, 0.000) INTO v_CurrenDebit FROM
						ExSyAccountsCurrency_CurrencymovementPruse AS a
				WHERE
						a.IsActive = 1
						AND (a.AccFrom = p_CurrencyID)

						AND a.BID = p_BranchID
						AND a.ID = p_ID LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
				IF v_CurrenCredit > v_CurrenDebit
					 THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) - v_CurrenCredit;
					END IF;
				IF v_CurrenCredit < v_CurrenDebit
					 THEN
SET v_NetVal = IFNULL(v_AccVal, 0.000) + v_CurrenDebit;
					END IF;
			END IF;




















		RETURN v_NetVal;
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SafeIDCur_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SafeIDEMP_GetNetTotal`;
DELIMITER //
CREATE FUNCTION `SafeIDEMP_GetNetTotal`(`p_AccName` BIGINT, `p_BranchID` INT, `p_CurrencyID` INT, `p_ID` BIGINT) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID bigint;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
		 if p_CurrencyID = 1 or p_CurrencyID = 13
		  THEN
		 SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID;
         SELECT IFNULL(SUM(a.debit), 0.000) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID;


		   SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID LIMIT 1;
         SELECT IFNULL(a.debit, 0.000) INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
		   IF v_CurrenCredit > v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) - v_CurrenCredit;
		   END IF;
		   IF v_CurrenCredit < v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) + v_CurrenDebit;
		   END IF;
		   END IF;




		    if p_CurrencyID <> 1
		  THEN
		 SELECT IFNULL(SUM(a.debit), 0.000) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID )
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID;
         SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID )
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID;


		   SELECT IFNULL(a.debit, 0.000) INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID)
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID LIMIT 1;
         SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID)
           AND a.AccIDFrom = p_AccName
           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
		   IF v_CurrenCredit > v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) - v_CurrenCredit;
		   END IF;
		   IF v_CurrenCredit < v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) + v_CurrenDebit;
		   END IF;
		   END IF;




















         RETURN v_NetVal;
     END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SafeIDEMP_GetNetTotal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SafeIDEMP_GetNetTotal1`;
DELIMITER //
CREATE FUNCTION `SafeIDEMP_GetNetTotal1`(`p_BranchID` INT, `p_CurrencyID` INT, `p_ID` BIGINT, `p_D1` DATE, `p_D2` DATE) RETURNS decimal(15,3)
    DETERMINISTIC
BEGIN
DECLARE v_AccCreditVal DECIMAL(15, 3);
DECLARE v_AccdebitVal DECIMAL(15, 3);
DECLARE v_FinalAccVal DECIMAL(15, 3);
DECLARE v_NetVal DECIMAL(15, 3);
DECLARE v_AccVal DECIMAL(15, 3);
DECLARE v_MaxID bigint;
DECLARE v_CurrenDebit DECIMAL(15, 3);
DECLARE v_CurrenCredit DECIMAL(15, 3);
		 if p_CurrencyID = 1 or p_CurrencyID = 13
		  THEN
		 SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccCreditVal FROM ExSyAccounts_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)

           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE);
         SELECT IFNULL(SUM(a.debit), 0.000) INTO v_AccdebitVal FROM ExSyAccounts_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)

           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE);


		   SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenCredit FROM ExSyAccounts_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)

           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE) LIMIT 1;
         SELECT IFNULL(a.debit, 0.000) INTO v_CurrenDebit FROM ExSyAccounts_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID OR a.CurrencyID	=13)

           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE) LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
		   IF v_CurrenCredit > v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) - v_CurrenCredit;
		   END IF;
		   IF v_CurrenCredit < v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) + v_CurrenDebit;
		   END IF;
		   END IF;




		    if p_CurrencyID <> 1
		  THEN
		 SELECT IFNULL(SUM(a.debit), 0.000) INTO v_AccCreditVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID )

           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE);
         SELECT IFNULL(SUM(a.Credit), 0.000) INTO v_AccdebitVal FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID )

           AND a.AccBranchID = p_BranchID
		   AND a.id<p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE);


		   SELECT IFNULL(a.debit, 0.000) INTO v_CurrenCredit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
          WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID)

           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE) LIMIT 1;
         SELECT IFNULL(a.Credit, 0.000) INTO v_CurrenDebit FROM ExSyAccountsCurrency_AccSafeActivityTb AS a
         WHERE a.IsActive = 1
           AND (a.CurrencyID   = p_CurrencyID)

           AND a.AccBranchID = p_BranchID
		   AND a.id=p_ID
		   AND a.InsertDate BETWEEN CAST(p_D1 AS DATE) AND CAST(p_D2 AS DATE) LIMIT 1;
SET v_AccVal = v_AccdebitVal - v_AccCreditVal;
		   IF v_CurrenCredit > v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) - v_CurrenCredit;
		   END IF;
		   IF v_CurrenCredit < v_CurrenDebit
		    THEN
SET v_NetVal =IFNULL(v_AccVal,0.000) + v_CurrenDebit;
		   END IF;
		   END IF;




















         RETURN v_NetVal;
     END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SafeIDEMP_GetNetTotal1` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SalaryCalc_BonusVal`;
DELIMITER //
CREATE FUNCTION `SalaryCalc_BonusVal`(`p_EMPID` INT) RETURNS decimal(12,3)
    DETERMINISTIC
BEGIN
DECLARE v_BonusVal DECIMAL(12, 3);

        SELECT a.INCVAL INTO v_BonusVal FROM IncreaseValTb AS a
        WHERE a.EMPID = p_EMPID LIMIT 1;
        IF v_BonusVal IS NULL
             THEN
SET v_BonusVal = 0.000;
        END IF;

         RETURN v_BonusVal;
     END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SalaryCalc_BonusVal` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `SalePrice_mo_Value`;
DELIMITER //
CREATE FUNCTION `SalePrice_mo_Value`(`p_CountryID` INT, `p_value` INT, `p_Type_ID_int` INT, `p_Type_form` INT) RETURNS float
    DETERMINISTIC
BEGIN
    DECLARE v_OUtValies      FLOAT;
    DECLARE v_SalePrice       DECIMAL(18,3);
    DECLARE v_maxVAle         DECIMAL(18,3);
    DECLARE v_VuleNu          INT;
    DECLARE v_DicCountfaloe   DECIMAL(18,3);
    DECLARE v_serevesteType_coun INT;

    SELECT IFNULL(SalePrice, 1) INTO v_SalePrice
    FROM NewCurrencyPriceOwnDetailsTb AS a
    INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID = b.ID
    INNER JOIN CountiresTb AS c ON b.CountryID = c.ID AND a.CurrencyIDTo = c.DefualtCurrency
    WHERE a.CurrencyIDFrom = 1
      AND b.PriceType = 2
      AND b.AccountType = 3
      AND b.CountryID = p_CountryID LIMIT 1;

    SET p_value = p_value * v_SalePrice;
    SELECT COUNT(ID) INTO v_serevesteType_coun FROM CATEGORYTYPESDETAILSTB AS a WHERE a.CATID = p_Type_ID_int;

    IF p_Type_form = 0 THEN
        IF v_serevesteType_coun > 0 THEN
            SELECT a.MaxValue INTO v_maxVAle FROM ExtTraServiceTypeTb AS a WHERE a.ID = p_Type_ID_int;

            IF p_value <= v_maxVAle THEN
                SELECT p_value - CASE WHEN a.RateType = 0 THEN a.DisVal ELSE p_value * a.DisVal END
                  INTO p_value
                FROM CATEGORYTYPESDETAILSTB AS a
                WHERE a.CATID = p_Type_ID_int AND p_value >= a.ValFrom AND p_value <= a.ValTo;
                SET v_OUtValies = p_value;
            END IF;

            IF p_value > v_maxVAle THEN
                SET v_VuleNu = FLOOR(p_value / v_maxVAle);
                SELECT CASE WHEN a.RateType = 0 THEN a.DisVal ELSE v_maxVAle * a.DisVal END
                  INTO v_DicCountfaloe
                FROM CATEGORYTYPESDETAILSTB AS a
                WHERE a.CATID = p_Type_ID_int AND v_maxVAle >= a.ValFrom AND v_maxVAle <= a.ValTo LIMIT 1;
                SET v_DicCountfaloe = v_DicCountfaloe * v_VuleNu;
                SET v_maxVAle = v_maxVAle * v_VuleNu;
                SET p_value = p_value - v_maxVAle;
                IF p_value > 0 THEN
                    SELECT p_value - CASE WHEN a.RateType = 0 THEN a.DisVal ELSE p_value * a.DisVal END
                      INTO p_value
                    FROM CATEGORYTYPESDETAILSTB AS a
                    WHERE a.CATID = p_Type_ID_int AND p_value >= a.ValFrom AND p_value <= a.ValTo;
                END IF;
                SET v_OUtValies = (v_maxVAle - v_DicCountfaloe) + p_value;
            END IF;
        END IF;
    END IF;

    RETURN IFNULL(v_OUtValies, p_value);
END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`SalePrice_mo_Value` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `sumcredet1`;
DELIMITER //
CREATE FUNCTION `sumcredet1`(`p_tybid` INT, `p_acccode` BIGINT, `p_date1` DATE, `p_date2` DATE, `p_CurrencyFrom` INT, `p_BranchID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_sumcredet FLOAT;
DECLARE v_CRUNSEFROM BIT;
DECLARE v_MainBranch INT;
DECLARE v_Ismain bit;

		SELECT co.ismain INTO v_Ismain FROM CoBranch as co where co.ID=p_BranchID LIMIT 1;
			SELECT co.id INTO v_MainBranch FROM CoBranch as co where co.ismain=1 LIMIT 1;
		SELECT IsDefault INTO v_CRUNSEFROM FROM
				CurrencyMainTb AS CMT
		WHERE
				id = p_CurrencyFrom
 ORDER BY CMT.ID DESC LIMIT 1;
		IF v_CRUNSEFROM = 1
			 THEN
			if v_Ismain=1
			 THEN
				SELECT IFNULL(SUM(Credit), 0) INTO v_sumcredet FROM
						ExSyAccounts_AccSafeActivityTb AS a
					INNER JOIN
						AccountsTb AS b
							ON b.AccID = a.AccIDFrom
				WHERE
						b.AccCode = p_acccode
						AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
						AND a.IsActive = 1
						AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
						AND a.AccBranchID=v_MainBranch;
			END IF;

			if v_Ismain<>1
			 THEN
				SELECT IFNULL(SUM(Credit), 0) INTO v_sumcredet FROM
						ExSyAccounts_AccSafeActivityTb AS a
					INNER JOIN
						AccountsTb AS b
							ON b.AccID = a.AccIDFrom
				WHERE
						b.AccCode = p_acccode
						AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
						AND a.IsActive = 1
						AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
						AND a.AccBranchID=p_BranchID;
			END IF;

					END IF;
		IF v_CRUNSEFROM = 0
			 THEN
				SELECT IFNULL(SUM(Credit), 0) INTO v_sumcredet FROM
						ExSyAccountsCurrency_AccSafeActivityTb AS a
					INNER JOIN
						AccountsTb AS b
							ON b.AccID = a.AccIDFrom
				WHERE
						b.AccCode = p_acccode
						AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
						AND a.IsActive = 1
						AND a.CurrencyID = p_CurrencyFrom;
			END IF;


		RETURN v_sumcredet;
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`sumcredet1` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `sumDEBETalter1`;
DELIMITER //
CREATE FUNCTION `sumDEBETalter1`(`p_tybid` INT, `p_acccode` BIGINT, `p_date1` DATE, `p_date2` DATE, `p_CurrencyFrom` INT, `p_BranchID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_debit FLOAT;
DECLARE v_CurrencyID BIT;
DECLARE v_MainBranch INT;
DECLARE v_Ismain bit;
		SELECT co.ismain INTO v_Ismain FROM CoBranch as co where co.ID=p_BranchID LIMIT 1;
			SELECT co.id INTO v_MainBranch FROM CoBranch as co where co.ismain=1 LIMIT 1;

		SELECT IsDefault INTO v_CurrencyID FROM
				CurrencyMainTb AS CMT WHERE CMT.ID=p_CurrencyFrom
 ORDER BY CMT.ID DESC LIMIT 1;
		IF v_CurrencyID = 1
			 THEN

				IF p_tybid = 0
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccounts_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccParent = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)

								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13);

					END IF;
				IF p_tybid = 1
					 THEN
					if v_Ismain=1
			 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccounts_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)

								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
								AND a.AccBranchID=v_MainBranch;
					END IF;
					if v_Ismain<>1
			 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccounts_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)

								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
								AND a.AccBranchID=p_BranchID;
					END IF;
					END IF;

			END IF;
		IF v_CurrencyID =0
			 THEN
				IF p_tybid = 0
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccountsCurrency_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccParent = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)

								AND a.IsActive = 1
								AND a.CurrencyID = p_CurrencyFrom;
					END IF;
				IF p_tybid = 1
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccountsCurrency_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)

								AND a.IsActive = 1
								AND a.CurrencyID = p_CurrencyFrom;
					END IF;

			END IF;


		RETURN v_debit;
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`sumDEBETalter1` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `sumDEBETalter2`;
DELIMITER //
CREATE FUNCTION `sumDEBETalter2`(`p_tybid` INT, `p_acccode` BIGINT, `p_date1` DATE, `p_date2` DATE, `p_CurrencyFrom` INT, `p_BranchID` INT) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE v_debit FLOAT;
DECLARE v_CurrencyID BIT;
DECLARE v_MainBranch INT;
DECLARE v_Ismain bit;
		SELECT co.ismain INTO v_Ismain FROM CoBranch as co where co.ID=p_BranchID LIMIT 1;
			SELECT co.id INTO v_MainBranch FROM CoBranch as co where co.ismain=1 LIMIT 1;

		SELECT IsDefault INTO v_CurrencyID FROM
				CurrencyMainTb AS CMT WHERE CMT.ID=p_CurrencyFrom
 ORDER BY CMT.ID DESC LIMIT 1;
		IF v_CurrencyID = 1
			 THEN

				IF p_tybid = 0
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccounts_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccParent = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13);

					END IF;
				IF p_tybid = 1
					 THEN
					if v_Ismain=1
			 THEN
						SELECT IFNULL(SUM(a.Debit), 0)+IFNULL(SUM(c.Debit), 0) INTO v_debit FROM
								ExSyAccounts_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
									INNER JOIN
								ExAssociationAct_AssActivityTb AS c
									ON c.AccIDFrom = a.AccIDFrom
									INNER JOIN
								AccountsTb AS d
									ON d.AccID = c.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND d.accid=p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
								AND a.AccBranchID=v_MainBranch;
					END IF;
					if v_Ismain<>1
			 THEN
						SELECT IFNULL(SUM(a.Debit), 0) INTO v_debit FROM
								ExSyAccounts_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom

						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND (a.CurrencyID=p_CurrencyFrom or a.CurrencyID=13)
								AND a.AccBranchID=p_BranchID;
					END IF;
					END IF;

			END IF;
		IF v_CurrencyID =0
			 THEN
				IF p_tybid = 0
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccountsCurrency_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom

						WHERE
								b.AccParent = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND a.CurrencyID = p_CurrencyFrom;
					END IF;
				IF p_tybid = 1
					 THEN
						SELECT IFNULL(SUM(Debit), 0) INTO v_debit FROM
								ExSyAccountsCurrency_AccSafeActivityTb AS a
							INNER JOIN
								AccountsTb AS b
									ON b.AccID = a.AccIDFrom
						WHERE
								b.AccCode = p_acccode
								AND a.InsertDate BETWEEN CAST(p_date1 AS DATE) AND CAST(p_date2 AS DATE)
								AND b.AccActive = 1
								AND a.IsActive = 1
								AND a.CurrencyID = p_CurrencyFrom;
					END IF;

			END IF;


		RETURN v_debit;
	END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`sumDEBETalter2` TO `exchange_app`@`%`;

DROP FUNCTION IF EXISTS `TestAccount_CanUseVal`;
DELIMITER //
CREATE FUNCTION `TestAccount_CanUseVal`(`p_AccID` BIGINT, `p_CurrencyID` INT, `p_IsBank` TINYINT UNSIGNED, `p_Val` DECIMAL(18,6)) RETURNS tinyint(1)
    DETERMINISTIC
BEGIN
DECLARE v_CanDepit BIT;
DECLARE v_IsLimted BIT;
DECLARE v_LimtedVal DECIMAL(18, 3);
DECLARE v_AccVal DECIMAL(18, 6);

    SELECT CASE                                 WHEN at.AccDmType = 0                                   AND at.AccParent NOT LIKE '101010%'                                    AND at.AccParent NOT LIKE '101020%'                                    AND at.AccParent NOT LIKE '101070%'                                    THEN                                   1                                 ELSE                                 at.CanDebit                         END, at.IsLimited, IFNULL(at.LimitedVal, 0) INTO v_CanDepit, v_IsLimted, v_LimtedVal FROM
            AccountsTb at
    WHERE
            at.AccID = p_AccID LIMIT 1;


    SET v_AccVal = IFNULL(Account_GetAccVal(p_AccID, p_CurrencyID, p_IsBank), 0);
    IF v_AccVal < p_Val
       THEN
        IF v_CanDepit = 0 OR p_IsBank=1
           THEN
            RETURN 0;
          END IF;

        IF v_CanDepit = 1
          AND
          v_IsLimted = 1
          AND
          (v_LimtedVal + v_AccVal) < p_Val
           THEN
            RETURN 0;
          END IF;

      END IF;


    RETURN 1;
  END//
DELIMITER ;
GRANT EXECUTE ON FUNCTION `exchangesys2026`.`TestAccount_CanUseVal` TO `exchange_app`@`%`;

FLUSH PRIVILEGES;
