﻿CREATE PROCEDURE [dbo].[spCart_Delete]
	@Id int
AS
Begin

	Delete from dbo.Cart where UserId = @Id

End
