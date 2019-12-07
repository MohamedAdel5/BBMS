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


select*from shift_manager
select*from login
insert into login (username,user_pass, user_type)VALUES ('emad_98',HASHBYTES('SHA2_512','124456'),'S')


INSERT INTO shift_manager(username,name) VALUES ('emad_98','omda')

delete from login where user_type='S'
delete shift_manager

update shift_manager set name='samy' where username='somya98'
update login set user_pass=HASHBYTES('SHA2_512','654321') where username='somya98'

select*from shift_manager  where username='maged98'


select*from blood_camp
select*from shift_manager
select* from shift