CREATE PROCEDURE [dbo].[spOrder_Update]
	@Id int,
	@CreatedDate datetime,
	@TotalAmount money
AS
Begin
	
	Update dbo.[Order]
	Set [CreatedDate] = @CreatedDate,
	TotalAmount = @TotalAmount
	Where Id = @Id

End
