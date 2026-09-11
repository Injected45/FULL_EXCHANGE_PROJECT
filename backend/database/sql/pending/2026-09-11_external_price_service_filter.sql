/* ══════════════════════════════════════════════════════════════════════════
 *  إصلاحُ سعرِ الحوالة الخارجية — سطرٌ واحد يُضاف، ولا حسابَ يتغيّر
 * ══════════════════════════════════════════════════════════════════════════
 *
 *  ⚠⚠ هذا الملفّ داخلَ الخطّ الأحمر الماليّ. لا يُنفَّذ إلّا بأمرِ المالك.
 *
 *  ── العطب ──────────────────────────────────────────────────────────────
 *
 *  نسخةُ استعلامِ السعر داخل المحفّز ينقصها شرطُ الخدمة (b.BankID)، وهو
 *  موجودٌ في دالّة المنظومة dbo.SalePrice_mo_Value التي تحسب NetTotal.
 *
 *  فلمصرَ خمسةُ صفوفٍ تتأهّل، و SELECT @SalePrice = … على مجموعةٍ متعدّدة
 *  تُبقي آخرَ صفٍّ يصله المنفّذ — سعراً اعتباطياً لا سعرَ الخدمة المختارة.
 *
 *  المقياسُ على القاعدة (11 سبتمبر 2026): تسعُ حوالاتٍ من عشرٍ كُتبت بـ5.300
 *  بينما سعرُ خدمتها المُدرج 5.550، وكلُّ واحدةٍ خرجت بعمولةِ خدمةٍ سالبة —
 *  أي أنّ الشركة سلّمت أكثر ممّا قيّدت، في كلّ حوالةٍ إلى مصر.
 *
 *  ── ما يتغيّر ──────────────────────────────────────────────────────────
 *
 *  سطرٌ واحدٌ يُضاف إلى شرطِ WHERE. لا معادلةَ تُمسّ، ولا عمودَ يُضاف، ولا
 *  قيدَ يُكتب، ولا صفَّ تاريخيٌّ يُعدَّل. والأثرُ على الإدراج الجديد وحدَه.
 *
 *  ── ⚠ شرطٌ قبل التنفيذ ─────────────────────────────────────────────────
 *
 *  كلُّ خدمةٍ معروضةٍ في ExtTraServiceTypeTb يجب أن يكون لها صفُّ سعرٍ في
 *  نطاق AppBriceTB. فخدمةٌ بلا سعرٍ كانت تأخذ سعرَ جارتها، وبعد الإصلاح
 *  يبقى @SalePrice فارغاً (NULL) فتُكتب TransPrice فارغة.
 *
 *  والفحصُ أدناه يمنع التنفيذ إن وُجدت ثغرة. المعلومُ اليوم: «بنكك» في
 *  السودان (خدمة 10) بلا صفِّ سعر — تُسعَّر من المكتب الخلفيّ أوّلاً.
 * ══════════════════════════════════════════════════════════════════════════ */

SET NOCOUNT ON;
GO

/* ── ١) لا خدمةَ معروضةٌ بلا سعر ─────────────────────────────────────── */
IF EXISTS (
    SELECT 1
    FROM ExtTraServiceTypeTb AS s
    WHERE NOT EXISTS (
        SELECT 1
        FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID = b.ID
        INNER JOIN CountiresTb AS c ON b.CountryID = c.ID AND a.CurrencyIDTo = c.DefualtCurrency
        INNER JOIN AppBriceTB AS e ON b.CountryID   = e.CountryID
                                  AND b.AccountType = e.AccountType
                                  AND b.BranchID    = e.BranchID
        WHERE a.CurrencyIDFrom = 1
          AND b.PriceType = 2
          AND b.CountryID = s.CountryID
          AND b.BankID    = s.ID
    )
)
BEGIN
    SELECT s.ID AS ServiceID, s.ServiceName, s.CountryID, N'بلا سعر معرَّف' AS Note
    FROM ExtTraServiceTypeTb AS s
    WHERE NOT EXISTS (
        SELECT 1
        FROM NewCurrencyPriceOwnDetailsTb AS a
        INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID = b.ID
        INNER JOIN CountiresTb AS c ON b.CountryID = c.ID AND a.CurrencyIDTo = c.DefualtCurrency
        INNER JOIN AppBriceTB AS e ON b.CountryID   = e.CountryID
                                  AND b.AccountType = e.AccountType
                                  AND b.BranchID    = e.BranchID
        WHERE a.CurrencyIDFrom = 1
          AND b.PriceType = 2
          AND b.CountryID = s.CountryID
          AND b.BankID    = s.ID
    );

    RAISERROR (N'توقّف: خدماتٌ معروضةٌ بلا سعر معرَّف. سعّرها من المكتب الخلفيّ ثمّ أعد التنفيذ.', 16, 1);
    SET NOEXEC ON;
END
GO

/* ── ٢) المحفّز، بالسطر المضاف ───────────────────────────────────────── */
ALTER TRIGGER [dbo].[ExternalEx_insert_Mobile] on  [dbo].[ExternalEx]
for 
insert 
as 
begin 
begin try 
begin transaction 

declare @Type_Moble as int 
select @Type_Moble = a.Type_Moble   from inserted  as  a
if @Type_Moble = 1 
begin 
--------------------------------------------------في حال تجواز القيمة المسموح فيها بناء علي معد تحويل من خلال الفتنرة ---------------------------------------------------
declare @Count_FromAnnull as bigint
set @Count_FromAnnull = isnull ((select  count(a.ACCID ) from GET_forDaily_transfer_preparer_schedule 
((select a.AccFrom from inserted as a  ), (select isnull (a.CurrRecievedVal ,0 )+ isnull (a.ExVal  , 0) from inserted as a  )) as a
where a.ISACtive = 1 ) ,0)

	
DECLARE @mOUNT AS FLOAT --------------------------- متغير يقوم بجلب الرصيد الحالي للعميل للتحقق من قيمة الرصيد 
DECLARE @VAlue as   FLOAT --------------------------- متغير يقوم بجلب الرصيد الحالي للعميل للتحقق من قيمة الرصيد 
DECLARE @type_ueser  as   FLOAT  ------------------التحقق من ان المرسل عميل او وكيل 
select @mOUNT = isnull (Sum(a.Credit) , 0 )  - isnull (sum(a.Debit) ,0) from  [dbo].[EX24AccSafeActivityTb] as a 
inner join inserted as b on a.AccIDFrom = b.AccFrom and a.IsActive = 1

select @VAlue= isnull (a.CurrRecievedVal , 0  ) + isnull ( a.ExVal  , 0 )  from inserted  as a 
------------ التحقق من ان قيمة التحويل ليس اكبر من الرصيد الحالي في حالة كانت المرسلة ال------------
 select @type_ueser = a.UeserType from users  as a inner join  inserted as b on a.AccID = b.AccFrom 


if @Count_FromAnnull > 0 
begin 

print ('تم تجاوز  الفترة المحددة')
rollback transaction 
return 

end 

declare @Rollback_from as int 
 if @type_ueser  = 3 or @type_ueser = 5
begin 
select @Rollback_from =  dbo.Rollback_Branch_Trinsfrim_me(b.CurrentAccID, b.BranchType, 1, a.CurrRecievedVal, b.ID) from inserted as a 
inner join CoBranch as b on a.RecievedBranchID = b.ID


if @Rollback_from = 0 
begin 
print ('تم تجاوز  الفترة المحددة')
rollback transaction 
return 

end 
end 




-- تعريف المتغير لتخزين وقت الإدخال القديم
DECLARE @InsertTime AS  bigint ;

-- الحصول على وقت الإدخال القديم (الأحدث بناءً على أكبر ID)


-- حساب الفرق بالدقائق وتعيينه إلى المتغير
SELECT @InsertTime = DATEDIFF(MINUTE, 
                           a.InsertDate ,  -- دمج التاريخ والوقت
                             GETDATE() ) -- الوقت الحالي
FROM 
    ExternalEx AS a
INNER JOIN 
    inserted AS b ON a.AccFrom = b.AccFrom
WHERE 
    a.IDCode = (SELECT MAX(a2.IDCode) 
            FROM ExternalEx AS a2 
            WHERE a2.AccFrom = b.AccFrom);

 ----في حالة كانت  عملية التحويل اقل من خمسة دقائق عدم التسجيل
if isnull (@InsertTime , 6 ) < =  1
begin


 print (' عذرا لايمكن تنفيذ هذه الحوالة الا بعد مرور 1 دقائق  ' + ' الوقت المتبقي هو ' + convert (nvarchar , 5-@InsertTime))
rollback transaction 
return
end 

--------------------------------------------------------------------------------------



declare   @IDCODE as int  ----- جلب ماكس كود
, @Code as nvarchar(max)  -----جلب كود العملية 


,@SalePrice as decimal(18,3) ----جلب سعر التحويل 
,@ServiceExVal AS  decimal(18,3) ----جلب عموالة التحويل اجمالي
select @IDCODE =  isnull (  max (a.IDCode )  ,0 ) + 1  from ExternalEx as a  

------------------------------جلب سعر التحويل الخارجي -------------------------------------------------------------------

-----------------جلب سعر السفر للتحويل -----------------------------------------------------------------------------
  select @SalePrice = ISnull ( SalePrice    , 1 )    
   FROM
              NewCurrencyPriceOwnDetailsTb AS a
              INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID=b.ID
			  inner join CountiresTb as c on b.CountryID =c.ID and a.CurrencyIDTo = c.DefualtCurrency
			  inner join inserted as D on C.ID=D.CountryIDTo
               INNER JOIN AppBriceTB E ON b.CountryID = E.CountryID
			  WHERE
              a.CurrencyIDFrom = 1
              AND b.PriceType=2           
			  and b.AccountType =  E.AccountType
			   AND b.CountryID=D.CountryIDTo 
                AND b.BranchID=e.BranchID
               /* ⚠ شرطُ الخدمة — أُضيف 11 سبتمبر 2026 بأمر المالك.
                  كان ناقصاً هنا وهو موجودٌ في dbo.SalePrice_mo_Value،
                  فكانت خمسةُ صفوفٍ تتأهّل لمصر ويبقى آخرُها اعتباطاً. */
               and b.BankID = D.ServiceType
               Declare @ACcform as int 
if @type_ueser = 5 
begin 
select @ACcform = 0
end 

if @type_ueser <> 5 
begin 
select @ACcform = 1
end 
-------------------------------------------------------------------------------------------
SELECT @ServiceExVal = (A.[CurrRecievedVal] *  @SalePrice) - (isnull (dbo.SalePrice_mo_Value(A.CountryIDTo , A.[CurrRecievedVal] , A.ServiceType ,isnull( a.IsPrivateAccount,0)),0))  FROM inserted AS A 
select @Code = CONVERT(NVARCHAR,C.CountryID)  + CONVERT(NVARCHAR,C.CityID)+ CONVERT(NVARCHAR,A.BranchID) + '2' +'-' +'55'+'-'+  CONVERT(NVARCHAR,@IDCODE)
from  AccountsTb as a inner join inserted as b on a.AccID = b .AccFrom 
INNER JOIN CoBranch AS C ON A.BranchID = C.ID
WHERE B.ID = B.ID
-----------------------جملة التعديل في جدول الحولات الخارجية-----------------------------------------------------------------------------------------------------------
UPDATE [dbo].[ExternalEx]
   SET [IDCode] =@IDCODE
   ,[Code] =@Code
   ,[SenderName] =b.[SenderName] 
   ,[Phone1] =b.[Phone1]
      ,[Phone2] =c.AccPhone
      ,[DeliveredCurrencyID] = d.DefualtCurrency 
	  ,[ServiceExVal] =@ServiceExVal  
	  ,[NetTotal] =isnull ( dbo.SalePrice_mo_Value(A.CountryIDTo , A.[CurrRecievedVal] , A.ServiceType , isnull (a.IsPrivateAccount,0)) ,0)
      ,[SafeRecievedID] =55 
	  ,[IsDelivered] = 0
  ,[IsConfirmed] =0
      ,[IsActive] = 1   
      ,[TransPrice] = @SalePrice
      ,[ConfirmedType] = 1
      ,[IsInOrOut] =0
      ,[CurrDeliveredVal] = (A.[CurrRecievedVal] *  @SalePrice )    
      ,[BankIDTo] = case when (select xx.Type_String from  ExtTraServiceTypeTb as  xx  where a.ServiceType = xx.ID  ) = 'bank'  then b.CityIDTo else 0  end 
	  ,CityIDTo = case when (select xx.Type_String from  ExtTraServiceTypeTb as  xx  where a.ServiceType = xx.ID  ) = 'bank'  then 0 else b.CityIDTo end 
      ,TransPrice1 = @SalePrice
      ,IsAccFrom  = @ACcform
	 from [dbo].[ExternalEx] as a 
	 inner join inserted as b on a.ID= b.ID and a.AccFrom= b.AccFrom
	 inner join AccountsTb as c on b.AccFrom= c.AccID
	 inner join CountiresTb  as  d on b.CountryIDTo = d.ID
	 WHERE A.ID= B.ID AND B.AccFrom= B.AccFrom
-----------------------------------------------------------------------------------------------------------
	







 ------------------------------------------------------التحقق من حساب الفرع الراسل -----------------------------------------------
  if @type_ueser  <> 3 
 begin 
Declare @CMDType as int  ---- جلب طبيعة الجساب في حالة كان الحساب مدين او دائن

select @CMDType= a.AccDmType from AccountsTb as a inner join inserted as b on a.AccID = b.AccFrom
where b.ID= b.ID
if @VAlue > @mOUNT and @CMDType <> 0
begin
print ('القيمة اكبر من الرصيد الحالي')
rollback transaction
return
end 



if @VAlue = 0  
begin
print ('عذرا لايمكن ان تكون القيمة بصفر')
rollback transaction
return

end 


-----------------------------------------خصم القيمة من جدول الحسبات العميل او حولات صادرة داخلية ---------------------------------------------------------


DECLARE @AccIDTo AS  BIGINT 
SELECT @AccIDTo = AccID FROM AccountsTb AS A 
INNER JOIN  inserted  AS B ON A.BranchID = B.RecievedBranchID
WHERE A.AccParent = 2010603 AND A.AccActive = 1 


---------------------اول عملية خصم القيمة من حساب الزبون -------------------------------------------
INSERT INTO [dbo].[EX24AccSafeActivityTb]
           ([SafeID],[Debit],[Credit] ,[InsertDate],[Description],[ISID],[IsActive],[TypeID],[OperationTypeID],[AccBranchID],[AccIDFrom]
		   ,[AccIDTo],[IsConfirmed],[IsCanceled]
  ,[MovementType],[CurrencyID],[DailyClosed],[SafeIDDailyClose],[Note],[SafeIDMovement]
       ,inesrtMobile  )

		 select 55  , a.CurrRecievedVal  ,  0 ,GETDATE() , 'حوالة من التطبيق ' ,@Code ,1 ,2,2 , a.RecievedBranchID ,A.AccFrom ,
		 @AccIDTo,0,0 ,' حوالة خارجية عبر التطبيق'  ,a.RecievedCurrencyID , 0,55,'لايوجد ملاحظات' , '',a.Type_Moble from inserted as a 
-----------------------------------------------------استخراك قيمة العمولة من حساب العميل--------------------------------------------------------------

INSERT INTO [dbo].[EX24AccSafeActivityTb]
           ([SafeID],[Debit],[Credit] ,[InsertDate],[Description],[ISID],[IsActive],[TypeID],[OperationTypeID],[AccBranchID],[AccIDFrom]
		   ,[AccIDTo],[IsConfirmed],[IsCanceled]
  ,[MovementType],[CurrencyID],[DailyClosed],[SafeIDDailyClose],[Note],[SafeIDMovement]
       ,inesrtMobile  )
		 select 55  , a.ExVal  ,  0 ,GETDATE() , 'عمولة تحويل' ,@Code ,1 ,94,94 , a.RecievedBranchID ,A.AccFrom ,
		 @AccIDTo,0,0 ,'عمولة تحويل'  ,a.RecievedCurrencyID , 0,55,'لايوجد ملاحظات' , '',a.Type_Moble from inserted as a 
		 ----------------------------------------------------------------------------------------------------------------------------
		 INSERT INTO [dbo].[EX24AccSafeActivityTb]
           ([SafeID]
           ,[Debit]
           ,[Credit]
           ,[InsertDate]
           ,[Description]
           ,[ISID]
           ,[IsActive]
           ,[TypeID]
           ,[OperationTypeID]
           ,[AccBranchID]
           ,[AccIDFrom]
           ,[AccIDTo]
           ,[IsConfirmed]
           ,[IsCanceled]
           ,[MovementType]
           ,[CurrencyID]
           ,[DailyClosed]
           ,[SafeIDDailyClose]
           ,[Note]
           ,[SafeIDMovement]
		  , inesrtMobile
         )

		 select 55   ,  0 ,a.CurrRecievedVal + a.ExVal  ,GETDATE() , 'حوالة من التطبيق ' ,@Code ,1 ,2,2 , a.RecievedBranchID ,@AccIDTo,A.AccFrom ,0,0
		 ,' حوالة خارجية عبر التطبيق'  ,a.RecievedCurrencyID ,  0 , 55,'لايوجد ملاحظات' , '' ,a.Type_Moble from inserted as a 

end 
end 
commit transaction 
end try 
begin catch 


rollback transaction 

end catch


end


GO

SET NOEXEC OFF;
GO

/* ── ٣) التحقّق بعد التنفيذ ──────────────────────────────────────────── */
IF OBJECT_DEFINITION(OBJECT_ID('dbo.ExternalEx_insert_Mobile')) LIKE '%b.BankID = D.ServiceType%'
    PRINT N'PASS: شرطُ الخدمة مطبَّقٌ في المحفّز.';
ELSE
    PRINT N'FAIL: الشرط غير موجود — راجع.';
GO