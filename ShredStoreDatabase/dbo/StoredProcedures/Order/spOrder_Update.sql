CREATE PROCEDURE [dbo].[spOrder_Update]
	@Id int,
	@Date date
AS
Begin
	
	Update dbo.[Order]
	Set [Date] = @Date
	Where Id = @Id

End
