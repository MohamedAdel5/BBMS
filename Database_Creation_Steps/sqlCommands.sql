INSERT INTO Hospital (username,hospital_name, phone, City, governorate)
            VALUES ('elsalama','elsalam',01112773653,'october','giza')

insert into login (username,user_pass, user_type)
            VALUES ('elsalama',HASHBYTES('SHA2_512','123456'),'H')

delete from hospital where username='elsalama'
select * from login
select * from  Hospital

UPDATE hospital
SET hospital_name = 'omeghyption',
    phone = 01312773653,
	City= 'saft',
	governorate='giza'
WHERE username='dsfds';

select *from blood_camp;
insert into blood_camp (hospital_id,driver_name) values(15,'omar')

update blood_camp set hospital_id=17 ,driver_name='samy'where hospital_id=15