USE [BBMS2]
GO
/****** Object:  StoredProcedure [dbo].[AddBloodBag]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE Procedure [dbo].[AddBloodBag] 	-- Add the parameters for the stored procedure here


@national_id bigint,
@blood_bag_date date,
@blood_camp_id int,
@hospital_id int,
@notes varchar(500),
@blood_type varchar(3)




AS
BEGIN
	
	insert into blood_bag values(@national_id,@blood_bag_date,@blood_camp_id,@hospital_id,@notes,@blood_type)
	if (@@ROWCOUNT > 0)	/*If the insertion is successful*/
	BEGIN
		UPDATE [donor]
		SET blood_type = @blood_type
		WHERE national_id = @national_id
	END
END

GO
/****** Object:  StoredProcedure [dbo].[AddBloodCamp]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddBloodCamp]
	-- Add the parameters for the stored procedure here
	@hospital_id int,
	@driver_name varchar(30)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into blood_camp values(@hospital_id, @driver_name)


END

GO
/****** Object:  StoredProcedure [dbo].[AddServiceToHospital]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddServiceToHospital] 
	-- Add the parameters for the stored procedure here
	@hospital_id int,
	@service_id int,
	@value int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Insert into hospital_provides_service values(@hospital_id,@service_id,@value)

END

GO
/****** Object:  StoredProcedure [dbo].[bestBloodCamp]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE	[dbo].[bestBloodCamp]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP 1 
	hospital.hospital_id, hospital.hospital_name, blood_camp.blood_camp_id, COUNT(*) AS num
	FROM blood_bag JOIN blood_camp ON blood_bag.blood_camp_id = blood_camp.blood_camp_id
	JOIN hospital ON blood_camp.hospital_id = hospital.hospital_id
	GROUP BY blood_camp.blood_camp_id, hospital.hospital_id, hospital.hospital_name
	ORDER BY COUNT(*) DESC
END


GO
/****** Object:  StoredProcedure [dbo].[changePassword]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[changePassword]
	@username varchar(30),
	@password nvarchar(50)
AS
BEGIN
	UPDATE [login]
	SET user_pass = HASHBYTES('SHA2_512', @password)
	WHERE username = @username
END

GO
/****** Object:  StoredProcedure [dbo].[checkAdmin]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[checkAdmin]
	-- Add the parameters for the stored procedure here
	@password nvarchar(50),
	@username varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    If EXISTS (
			SELECT * 
			FROM [login] 
			WHERE 
			[user_pass] = HASHBYTES('SHA2_512', @password) 
			AND 
			user_type = 'A'
			AND
			[username] = @username
			)
	BEGIN
		SELECT * FROM 
		[login]
		WHERE [login].username = @username
	END
END


GO
/****** Object:  StoredProcedure [dbo].[CheckHospital]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CheckHospital] 
	-- Add the parameters for the stored procedure here
@password nvarchar(50),
@username varchar(30)

AS
BEGIN
If EXISTS (
			SELECT * 
			FROM [login] 
			WHERE 
			[user_pass] = HASHBYTES('SHA2_512', @password) 
			AND 
			user_type = 'H'
			AND
			[username] = @username
			)
	BEGIN
		SELECT * FROM 
		[login] AS l JOIN [hospital] AS h ON l.[username] = h.[username] 
		WHERE l.username = @username
	END
END

GO
/****** Object:  StoredProcedure [dbo].[checkNationalID]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[checkNationalID]
@n_id bigint
AS
BEGIN
		SELECT national_id
		FROM donor
		WHERE national_id = @n_id
    
END

GO
/****** Object:  StoredProcedure [dbo].[checkShiftManager]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[checkShiftManager]
	@password nvarchar(50),
	@username varchar(30)
AS
BEGIN
	If EXISTS (
			SELECT * 
			FROM [login] 
			WHERE 
			[user_pass] = HASHBYTES('SHA2_512', @password) 
			AND 
			user_type = 'S'
			AND
			[username] = @username
			)
	BEGIN
		SELECT * FROM 
		[login] AS l JOIN [shift_manager] AS sm ON l.[username] = sm.[username]
		WHERE l.username = @username
	END
END

GO
/****** Object:  StoredProcedure [dbo].[checkUser]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[checkUser]
@password nvarchar(50),
@username varchar(30)
AS
BEGIN
If EXISTS (
			SELECT * 
			FROM [login] 
			WHERE 
			[user_pass] = HASHBYTES('SHA2_512', @password) 
			AND 
			user_type = 'U'
			AND
			[username] = @username
			)
	BEGIN
		SELECT * FROM 
		[login] AS l JOIN [user] AS u ON l.[username] = u.[username] 
		JOIN [donor] AS d ON u.[national_id] = d.[national_id]
		WHERE l.username = @username
	END
END

GO
/****** Object:  StoredProcedure [dbo].[checkUsername]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[checkUsername]
@username varchar(30)
AS
BEGIN
		SELECT username
		FROM [login]
		WHERE username = @username

END


GO
/****** Object:  StoredProcedure [dbo].[checkUsernameIsAdmin]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE  [dbo].[checkUsernameIsAdmin]
@username varchar(30)
AS
BEGIN
		SELECT username
		FROM [login]
		WHERE username = @username
		AND user_type = 'A'


END


GO
/****** Object:  StoredProcedure [dbo].[checkUsernameIsShiftManager]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE  [dbo].[checkUsernameIsShiftManager]
@username varchar(30)
AS
BEGIN
		SELECT username
		FROM [login]
		WHERE username = @username
		AND user_type = 'S'


END


GO
/****** Object:  StoredProcedure [dbo].[checkUsernameIsUser]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE  [dbo].[checkUsernameIsUser]
@username varchar(30)
AS
BEGIN
		SELECT username
		FROM [login]
		WHERE username = @username
		AND user_type = 'U'


END


GO
/****** Object:  StoredProcedure [dbo].[consumeService]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[consumeService]
	@username varchar(30),
	@service_id int,
	@hospital_id int,
	@service_value int
AS
BEGIN
	DECLARE @user_nID bigint
	SET @user_nID = (SELECT [national_id] FROM [user] WHERE username = @username);

	UPDATE [user]
	SET points = points - @service_value
	WHERE username = @username
	
	INSERT INTO [user_uses_service](national_id,hospital_id,service_id_s,service_use_date)
	VALUES (@user_nID, @hospital_id, @service_id, CURRENT_TIMESTAMP)
END

GO
/****** Object:  StoredProcedure [dbo].[deleteHospital]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deleteHospital] 
	@h_id INT
AS
BEGIN
	DELETE FROM [blood_bag]  WHERE hospital_id = @h_id;
	DELETE FROM [shift] WHERE blood_camp_id IN
	(
		SELECT blood_camp_id 
		FROM blood_camp
		WHERE hospital_id = @h_id
	)
	DELETE FROM [blood_camp] WHERE hospital_id = @h_id;
	DELETE FROM [shift_manager] WHERE hospital_id = @h_id;


	DELETE FROM [login] WHERE [login].username = (SELECT username FROM hospital WHERE hospital_id = @h_id)
	/*Then the delete cascade property will delete the hospital*/
END

GO
/****** Object:  StoredProcedure [dbo].[deleteService]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[deleteService]
	@service_id INT
AS
BEGIN
	DELETE FROM [service] WHERE service_id = @service_id
END

GO
/****** Object:  StoredProcedure [dbo].[getBBagsNums]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getBBagsNums]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [BBMS2].[dbo].[blood_bag].[blood_type], COUNT(*) AS Num
	FROM [BBMS2].[dbo].[blood_bag]
	GROUP BY [BBMS2].[dbo].[blood_bag].[blood_type]
END


GO
/****** Object:  StoredProcedure [dbo].[getBloodBagsofType]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getBloodBagsofType]
	@blood_type varchar(3),
	@hospital_id int
AS
BEGIN
	SET NOCOUNT ON;

    if(@blood_type = 'all')
	begin

	SELECT b.blood_type,b.blood_bag_id,b.blood_camp_id,b.blood_bag_date,b.national_id,b.notes
	from blood_bag as b
	where b.hospital_id = @hospital_id
	order by b.blood_type asc, b.blood_bag_date desc

	
	end

	else
	begin
	
	SELECT b.blood_type,b.blood_bag_id,b.blood_camp_id,b.blood_bag_date,b.national_id,b.notes
	from blood_bag as b
	where b.hospital_id = @hospital_id and b.blood_type = @blood_type
	order by b.blood_bag_date desc
	
	end
END
GO
/****** Object:  StoredProcedure [dbo].[GetBloodCamps]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetBloodCamps]
	-- Add the parameters for the stored procedure here
	@hospital_id int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select blood_camp.blood_camp_id, blood_camp.driver_name
	from blood_camp
	where blood_camp.hospital_id = @hospital_id
END

GO
/****** Object:  StoredProcedure [dbo].[getBloodCampsAndShifts]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getBloodCampsAndShifts]

AS
BEGIN
	SELECT BC.blood_camp_id, 
	H.hospital_name,  
	S.shift_date, 
	S.start_hour, 
	S.finish_hour, 
	S.governorate, 
	S.city,
	SM.name, 
	BC.driver_name 
	FROM blood_camp AS BC JOIN [shift] AS S ON BC.blood_camp_id = S.blood_camp_id
	JOIN hospital AS H ON H.hospital_id = BC.hospital_id
	JOIN shift_manager AS SM ON SM.username = S.shift_manager_username
END

GO
/****** Object:  StoredProcedure [dbo].[getBloodCampsDetails]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getBloodCampsDetails] 
	-- Add the parameters for the stored procedure here
	@hospital_id int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	from blood_camp left outer join shift
	on blood_camp.blood_camp_id = shift.blood_camp_id
	where blood_camp.hospital_id = @hospital_id
	order by blood_camp.blood_camp_id
END

GO
/****** Object:  StoredProcedure [dbo].[getCampShifts]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getCampShifts] 
	-- Add the parameters for the stored procedure here
@blood_camp_id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	from shift
	where blood_camp_id = @blood_camp_id

END

GO
/****** Object:  StoredProcedure [dbo].[getHospitals]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getHospitals]
AS
BEGIN
	
	SELECT * FROM hospital
END

GO
/****** Object:  StoredProcedure [dbo].[getHospitalService]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getHospitalService]
@hospital_id int,
@service_id int
AS
BEGIN
	
	SELECT * FROM [hospital_provides_service] WHERE hospital_id = @hospital_id AND service_id_p = @service_id

END

GO
/****** Object:  StoredProcedure [dbo].[getServices]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getServices]
AS
BEGIN
	
	SELECT s.name, h.hospital_name, hps.value
	FROM [hospital_provides_service] AS hps
	JOIN [hospital] AS h ON h.hospital_id = hps.hospital_id
	JOIN [service] AS s ON s.service_id = hps.service_id_p

END

GO
/****** Object:  StoredProcedure [dbo].[getServicesNamesAndIDs]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getServicesNamesAndIDs]
AS
BEGIN
	
	SELECT name, service_id FROM [service]

END

GO
/****** Object:  StoredProcedure [dbo].[getShiftManagers]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getShiftManagers] 
	-- Add the parameters for the stored procedure here
@hospital_id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT m.username,m.name
	from shift_manager as m
	where m.hospital_id = @hospital_id
END

GO
/****** Object:  StoredProcedure [dbo].[getTopUsers]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[getTopUsers]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 10
       [name]
      ,[donation_count]
  FROM [BBMS2].[dbo].[user] JOIN [BBMS2].[dbo].[donor] ON [BBMS2].[dbo].[donor].[national_id] = [BBMS2].[dbo].[user].[national_id]
  ORDER BY	[BBMS2].[dbo].[user].[donation_count] DESC
END


GO
/****** Object:  StoredProcedure [dbo].[getUser]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getUser]
	@username varchar(30)
AS
BEGIN
	SELECT * FROM 
		[login] AS l JOIN [user] AS u ON l.[username] = u.[username] 
		JOIN [donor] AS d ON u.[national_id] = d.[national_id]
		WHERE l.username = @username
END

GO
/****** Object:  StoredProcedure [dbo].[getUserDonations]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getUserDonations] 
	@national_id bigint
AS
BEGIN
	SELECT bb.blood_bag_date, h.hospital_name
	FROM blood_bag AS bb JOIN hospital AS h ON bb.hospital_id = h.hospital_id
	WHERE bb.national_id = @national_id
END


GO
/****** Object:  StoredProcedure [dbo].[getUserHealthInfo]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getUserHealthInfo] 
	@national_id bigint
AS
BEGIN
	SELECT blood_pressure, glucose_level, notes, report_date
	FROM donor_health_info
	WHERE national_id = @national_id
END


GO
/****** Object:  StoredProcedure [dbo].[getUserNotifications]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getUserNotifications]
	@username varchar(30)
AS
BEGIN
	
	SELECT [info], [date] FROM [notifications] WHERE username = @username
END

GO
/****** Object:  StoredProcedure [dbo].[getUserPoints]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getUserPoints]
	@username varchar(30)
AS
BEGIN
	
	SELECT points FROM [user] WHERE username = @username
	
END

GO
/****** Object:  StoredProcedure [dbo].[getUserServices]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getUserServices] 
	@national_id bigint
AS
BEGIN
	SELECT s.name, uus.service_use_date, h.hospital_name, hps.value
	FROM user_uses_service AS uus 
		 JOIN hospital AS h ON uus.hospital_id = h.hospital_id
		 JOIN hospital_provides_service AS hps ON (uus.hospital_id = hps.hospital_id AND uus.[service_id_s] = hps.[service_id_p])
		 JOIN [service] AS s ON s.service_id = uus.service_id_s
	WHERE uus.national_id = @national_id
END

GO
/****** Object:  StoredProcedure [dbo].[getUsersOfBloodType]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getUsersOfBloodType]
	@blood_type varchar(3)
AS
BEGIN
	
	SELECT username FROM
	[user] AS u JOIN [donor] AS d
	ON u.national_id = d.national_id
	WHERE d.blood_type = @blood_type

END

GO
/****** Object:  StoredProcedure [dbo].[getVolunteerHealthInfo]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getVolunteerHealthInfo]
	@n_id bigint,
	@h_id int
AS
BEGIN
	
	SELECT * FROM donor AS D 
	JOIN donor_health_info AS DHI ON D.national_id = DHI.national_id 
	WHERE D.national_id = @n_id AND DHI.hospital_id = @h_id
END

GO
/****** Object:  StoredProcedure [dbo].[GrantUserPoints]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GrantUserPoints]
	@username varchar(30),
	@points int
AS
BEGIN

DECLARE @current_points INT;
DECLARE @current_donations INT;


SET @current_points = (SELECT [points] FROM [user] WHERE username = @username);
SET @current_donations = (SELECT [donation_count] FROM [user] WHERE username = @username);


UPDATE [user]
SET [points]		 = @current_points + @points,
	[donation_count] = @current_donations + 1
WHERE username = @username

END

GO
/****** Object:  StoredProcedure [dbo].[HospitalNotProvideServices]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[HospitalNotProvideServices]
	-- Add the parameters for the stored procedure here
	 @hospital_id int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT s1.name, s1.service_id
	FROM	service as s1
	WHERE s1.service_id not in(
	
	SELECT s2.service_id
	FROM	service as s2, hospital_provides_service as h2
	WHERE	s2.service_id = h2.service_id_p	AND hospital_id = @hospital_id
	)
END

GO
/****** Object:  StoredProcedure [dbo].[HospitalProvideServices]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HospitalProvideServices]
	-- Add the parameters for the stored procedure here
	 @hospital_id int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT s.service_id,s.name, h.value
	FROM	service as s, hospital_provides_service as h
	WHERE	s.service_id = h.service_id_p	AND h.hospital_id = @hospital_id
END

GO
/****** Object:  StoredProcedure [dbo].[insert_admin]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------

CREATE PROCEDURE [dbo].[insert_admin]
	@username varchar(30),
	@user_pass nvarchar(50)

AS
BEGIN
IF (
	NOT EXISTS (SELECT * FROM [login] WHERE username = @username)
	)
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
	END

END;


GO
/****** Object:  StoredProcedure [dbo].[insert_blood_camp]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------

CREATE PROCEDURE [dbo].[insert_blood_camp]
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
/****** Object:  StoredProcedure [dbo].[insert_donorHealthInfo]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[insert_donorHealthInfo] 
	@hospital_id int,
	@national_id bigint,
	@report_date date,
	@blood_pressure varchar(10),
	@glucose_level varchar(10),
	@notes varchar(500)
AS
BEGIN
	INSERT INTO donor_health_info 
	VALUES
	(
		@national_id,
		@hospital_id,
		@report_date,
		@blood_pressure,
		@glucose_level,
		@notes
	)
END

GO
/****** Object:  StoredProcedure [dbo].[insert_hospital]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------

CREATE PROCEDURE [dbo].[insert_hospital]
	@username varchar(30),
	@user_pass nvarchar(50),
	@hospital_name varchar(30),
	@phone bigint,
	@city varchar(30),
	@governorate varchar(30)

AS
BEGIN
IF(
	NOT EXISTS (SELECT * FROM [login] WHERE username = @username)
	AND NOT EXISTS (SELECT * FROM [hospital] WHERE phone = @phone)
  )
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
		username,
		hospital_name,
		phone,
		city,
		governorate
	)
	VALUES
	(
		@username,
		@hospital_name,
		@phone,
		@city,
		@governorate
	)
   END
END;

GO
/****** Object:  StoredProcedure [dbo].[insert_service]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------

CREATE PROCEDURE [dbo].[insert_service]
	@name varchar(30)

AS
BEGIN
IF(
		NOT EXISTS(SELECT * FROM [service] WHERE name = @name)
  )
	BEGIN
		INSERT INTO [service]
		(
			name
		)
		VALUES
		(
			@name
		)
	END
	

END;


GO
/****** Object:  StoredProcedure [dbo].[insert_shift]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[insert_shift] 
-- Add the parameters for the stored procedure here
	
	@blood_camp_id int,	
	@shift_date date,
    @shift_manager_username varchar(30),
    @start_hour time(7),
    @finish_hour time(7),
    @city varchar(30),
    @governorate varchar(30)


AS
BEGIN
	
	DECLARE @sm_h_id INT;
	DECLARE @bc_h_id INT;

	SET @sm_h_id = (SELECT hospital_id FROM [shift_manager] WHERE username = @shift_manager_username);
	SET @bc_h_id = (SELECT hospital_id FROM [blood_camp] WHERE blood_camp_id = @blood_camp_id);

    -- Insert statements for procedure here
	IF(@sm_h_id = @bc_h_id)
	BEGIN
		insert into shift values(@blood_camp_id, @shift_date, @shift_manager_username, @start_hour, @finish_hour, @city, @governorate)		
	END

END

GO
/****** Object:  StoredProcedure [dbo].[insert_shift_manager]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------

CREATE PROCEDURE [dbo].[insert_shift_manager]
	@username varchar(30),
	@user_pass nvarchar(50),
	@name varchar(30),
	@hospital_id int
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
		username,
		name,
		hospital_id
	)
	VALUES
	(
		@username,
		@name,
		@hospital_id
	)

END;
GO
/****** Object:  StoredProcedure [dbo].[insert_user]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[insert_shift_manager]    Script Date: 15-Dec-19 12:41:09 AM ******/
CREATE PROCEDURE [dbo].[insert_user]
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
IF (
	NOT EXISTS (SELECT * FROM [login] WHERE username = @username)
	AND NOT EXISTS (SELECT * FROM [donor] WHERE phone = @phone)
	AND NOT EXISTS (SELECT * FROM [donor] WHERE national_id = @national_id)
	)
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
	END
END;
GO
/****** Object:  StoredProcedure [dbo].[insert_volunteer]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------

CREATE PROCEDURE [dbo].[insert_volunteer]
	@national_id bigint,
	@name varchar(30),
	@gender char(1),
	@age tinyint,
	@phone varchar(11),
	@city varchar(30),
	@governorate varchar(30)

AS
BEGIN
	IF (
		NOT EXISTS (SELECT * FROM [donor] WHERE phone = @phone)
		AND NOT EXISTS (SELECT * FROM [donor] WHERE national_id = @national_id)
		)
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
	END

END;


GO
/****** Object:  StoredProcedure [dbo].[mostHospitalUsed]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[mostHospitalUsed]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 10 
	[BBMS2].[dbo].[hospital].[hospital_name],COUNT(*) AS num
	FROM [BBMS2].[dbo].[hospital]
	JOIN [BBMS2].[dbo].[user_uses_service] ON [BBMS2].[dbo].[hospital].[hospital_id] = [BBMS2].[dbo].[user_uses_service].[hospital_id]
	GROUP BY [BBMS2].[dbo].[hospital].[hospital_id],[BBMS2].[dbo].[hospital].[hospital_name] 
	ORDER BY COUNT(*) DESC
END


GO
/****** Object:  StoredProcedure [dbo].[RemoveBloodBag]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE Procedure [dbo].[RemoveBloodBag] 	-- Add the parameters for the stored procedure here
@blood_bag_id int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from blood_bag where blood_bag.blood_bag_id = @blood_bag_id
END

GO
/****** Object:  StoredProcedure [dbo].[RemoveBloodCamp]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[RemoveBloodCamp]
	-- Add the parameters for the stored procedure here
@blood_camp_id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE from blood_camp where blood_camp.blood_camp_id = @blood_camp_id
END

GO
/****** Object:  StoredProcedure [dbo].[RemoveServiceFromHospital]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveServiceFromHospital]
	-- Add the parameters for the stored procedure here
@hospital_id int,
@service_id int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Delete from hospital_provides_service
	where hospital_id = @hospital_id AND service_id_p = @service_id
	
END

GO
/****** Object:  StoredProcedure [dbo].[RemoveShift]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveShift] 
	-- Add the parameters for the stored procedure here
@blood_camp_id int,
@shift_date date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE from shift where shift.blood_camp_id = @blood_camp_id AND shift.shift_date = @shift_date
END

GO
/****** Object:  StoredProcedure [dbo].[RemoveShiftManager]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveShiftManager]
	-- Add the parameters for the stored procedure here
	
	@username varchar(30)

AS
BEGIN

	SET NOCOUNT ON;
	
	UPDATE shift
	SET shift.shift_manager_username = null
	WHERE shift_manager_username = @username;

    delete from login  where login.username = @username and login.user_type = 'S'
	
END

GO
/****** Object:  StoredProcedure [dbo].[sendNotifications]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sendNotifications]
	@username varchar(30),
	@info varchar(500)
AS
BEGIN
	INSERT INTO notifications(username,info, [date]) 
	VALUES (@username, @info, CURRENT_TIMESTAMP)
	
END

GO
/****** Object:  StoredProcedure [dbo].[ShiftManagerExist]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ShiftManagerExist] 
	-- Add the parameters for the stored procedure here
@username varchar(30),
@hospital_id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT count(*) from shift_manager as m
	where hospital_id = @hospital_id
	and m.username = @username

END

GO
/****** Object:  StoredProcedure [dbo].[topUsersUseService]    Script Date: 22-Dec-19 2:36:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[topUsersUseService]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 10 
	[BBMS2].[dbo].[donor].[name],COUNT(*) AS num
	FROM [BBMS2].[dbo].[donor] join [BBMS2].[dbo].[user] ON [BBMS2].[dbo].[donor].[national_id] = [BBMS2].[dbo].[user].[national_id]
	JOIN [BBMS2].[dbo].[user_uses_service] ON [BBMS2].[dbo].[user].[national_id] = [BBMS2].[dbo].[user_uses_service].[national_id]
	GROUP BY [BBMS2].[dbo].[user].[username],[BBMS2].[dbo].[donor].[name] 
	ORDER BY COUNT(*) DESC
END


GO
