USE BBMS2
GO
CREATE PROCEDURE getServices
AS
BEGIN
	
	SELECT s.name, h.hospital_name, hps.value
	FROM [hospital_provides_service] AS hps
	JOIN [hospital] AS h ON h.hospital_id = hps.hospital_id
	JOIN [service] AS s ON s.service_id = hps.service_id_p

END
GO
