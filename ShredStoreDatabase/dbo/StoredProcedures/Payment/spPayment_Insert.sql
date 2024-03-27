CREATE PROCEDURE [dbo].[spPayment_Insert]
	@Amount money,
	@Date datetime,
	@PaymentType tinyint,
	@Payed bit
As
Begin
	
	Insert into dbo.Payment (Amount, [Date], PaymentType, Payed)
	Values (@Amount, @Date, @PaymentType, @Payed);

End
