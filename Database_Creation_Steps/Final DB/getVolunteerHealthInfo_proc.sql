
CREATE PROCEDURE getVolunteerHealthInfo
	@n_id bigint,
	@h_id int
AS
BEGIN
	
	SELECT * FROM donor AS D 
	JOIN donor_health_info AS DHI ON D.national_id = DHI.national_id 
	WHERE D.national_id = @n_id AND DHI.hospital_id = @h_id
END
GO
