CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id int
AS
Begin
	
	Exec spCartItem_DeleteAll @Id;
	Exec spCart_Delete @Id;
	Exec spOrder_DeleteUserOrders @Id;

	Delete from dbo.[User] where Id = @Id
	
End	
