CREATE PROCEDURE [dbo].[spOrder_Update]
	@Id int,
	@Date datetime
AS
Begin
	
	Update dbo.[Order]
	Set [Date] = @Date
	Where Id = @Id

End
