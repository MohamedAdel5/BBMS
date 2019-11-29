---------------------------------------------------Creating Procedures-----------------------------------------------------

CREATE PROCEDURE insert_user
	@username varchar(30),
	@user_pass nvarchar(50),
	@national_id bigint,
	@name varchar(30),
	@gender char(1),
	@age tinyint,
	@phone varchar(11),
	@city varchar(30),
	@governorate varchar(30)

AS
BEGIN
	INSERT INTO [login]
	(
		username,
		user_pass,
		user_type
	)
	VALUES
	(
		@username, 
		HASHBYTES('SHA2_512', @user_pass),
		'U'
	)

	INSERT INTO [donor]
	(
		national_id, 
		name, 
		gender,
		age,
		phone,
		city,
		governorate
	)
	VALUES
	(
		@national_id, 
		@name, 
		@gender,
		@age,
		@phone,
		@city,
		@governorate
	)

	INSERT INTO [user]
	(
		national_id,
		username,
		points
	)
	VALUES
	(
		@national_id,
		@username,
		0
	)

END;

GO
-------------------------------------------------------------

CREATE PROCEDURE insert_volunteer
	@national_id bigint,
	@name varchar(30),
	@gender char(1),
	@age tinyint,
	@phone bigint,
	@city varchar(30),
	@governorate varchar(30)

AS
BEGIN

	INSERT INTO [donor]
	(
		national_id, 
		name, 
		gender,
		age,
		phone,
		city,
		governorate
	)
	VALUES
	(
		@national_id, 
		@name, 
		@gender,
		@age,
		@phone,
		@city,
		@governorate
	)

END;
GO
-------------------------------------------------------------

CREATE PROCEDURE insert_hospital
	@username varchar(30),
	@user_pass nvarchar(50),
	@hospital_name varchar(30),
	@phone varchar(11),
	@city varchar(30),
	@governorate varchar(30)

AS
BEGIN
	INSERT INTO [login]
	(
		username,
		user_pass,
		user_type
	)
	VALUES
	(
		@username, 
		HASHBYTES('SHA2_512', @user_pass),
		'H'
	)

	INSERT INTO [hospital]
	(
		hospital_name,
		phone,
		city,
		governorate
	)
	VALUES
	(
		@hospital_name,
		@phone,
		@city,
		@governorate
	)

END;
GO
---------------------------------------------------------------

CREATE PROCEDURE insert_admin
	@username varchar(30),
	@user_pass nvarchar(50)

AS
BEGIN
	INSERT INTO [login]
	(
		username,
		user_pass,
		user_type
	)
	VALUES
	(
		@username, 
		HASHBYTES('SHA2_512', @user_pass),
		'A'
	)

END;
GO
---------------------------------------------------------------

CREATE PROCEDURE insert_blood_camp
	@hospital_id int,
	@driver_name varchar(30)

AS
BEGIN
	INSERT INTO [blood_camp]
	(
		hospital_id,
		driver_name
	)
	VALUES
	(
		@hospital_id,
		@driver_name
	)

END;
GO
------------------------------------------------------------------

CREATE PROCEDURE insert_shift_manager
	@username varchar(30),
	@user_pass nvarchar(50),
	@name varchar(30)

AS
BEGIN
	INSERT INTO [login]
	(
		username,
		user_pass,
		user_type
	)
	VALUES
	(
		@username, 
		HASHBYTES('SHA2_512', @user_pass),
		'S'
	)
	INSERT INTO [shift_manager]
	(
		name
	)
	VALUES
	(
		@name
	)

END;
GO
------------------------------------------------------------------

CREATE PROCEDURE [insert_service]
	@name varchar(30)

AS
BEGIN
	INSERT INTO [service]
	(
		name
	)
	VALUES
	(
		@name
	)
	

END;
GO