USE [BBMS2]
GO
/****** Object:  StoredProcedure [dbo].[RemoveShift]    Script Date: 22-Dec-19 1:54:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[RemoveShift] 
	-- Add the parameters for the stored procedure here
@blood_camp_id int,
@shift_date date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE blood_bag
	SET blood_bag_date = NULL,
		blood_camp_id = NULL
	WHERE blood_camp_id = @blood_camp_id

    -- Insert statements for procedure here
	DELETE from shift where shift.blood_camp_id = @blood_camp_id AND shift.shift_date = @shift_date
END
