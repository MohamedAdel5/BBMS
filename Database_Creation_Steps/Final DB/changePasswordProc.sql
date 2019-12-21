USE BBMS2
GO

CREATE PROCEDURE changePassword
	@username varchar(30),
	@password nvarchar(50)
AS
BEGIN
	UPDATE [login]
	SET user_pass = HASHBYTES('SHA2_512', @password)
	WHERE username = @username
END
GO
