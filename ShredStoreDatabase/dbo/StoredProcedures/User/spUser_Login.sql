﻿CREATE PROCEDURE [dbo].[spUser_Login]
	@Email nvarchar(50),
	@Password nvarchar(50),
	@ResponseMessage nvarchar(250)='' Output

AS
Begin
	
	SET NOCOUNT ON

	Declare @SelectedId int

	IF Exists (Select Top 1 Id From dbo.[User] Where [Email] = @Email)
	Begin
		Set @SelectedId=(Select Id from dbo.[User] Where [Email] = @Email And Password=HASHBYTES('SHA2_512',@Password+Cast(Salt As nvarchar(36))))

		If(@SelectedId Is Null)
			Set @ResponseMessage='Incorret Password'
		Else
			Set @ResponseMessage='Login Successfull'
			exec dbo.spUser_GetById @SelectedId
			
	End
	Else
		Set @ResponseMessage='Invalid Login Attempt'

End
	