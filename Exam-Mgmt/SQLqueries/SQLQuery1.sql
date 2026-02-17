select * from Course_Master where Obsolete = 'N';

alter table Course_Master
add Created_Date datetime default getdate(),
Created_By int not null,
Modified_Date datetime,
Modified_By int,;

insert into Course_Master (Course_Name,Created_By)
values ('First Year',1),('Second Year',1),('Third Year',1),('Fourth Year',1);

