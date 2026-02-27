create table Result_Master(
Res_Id int primary key identity(1,1),
Course_Id int not null,
Sem_Id int not null,
Student_Id int not null,
Exam_Id int not null,
Subject_Id int not null,
Obtained_Marks int not null,
Total_Marks int not null,
Created_By int not null,
Created_Date datetime not null default getdate(),
Modified_By int null,
Modified_Date datetime null,
Longitude decimal(18, 2) not null,
Latitude decimal(18, 2) not null,
Obsolete char(1) not null default 'N'
)


go
create proc sp_ResultCRUD
@Mode varchar(10) ,
@Res_Id int = null,
@Course_Id int = null,
@Sem_Id int = null,
@Student_Id int = null,
@Exam_Id int = null,
@Subject_Id int = null,
@Obtained_Marks int = null,
@Total_Marks int = null,
@Created_By int = null,
@Modified_By int = null,
@Modified_Date datetime = null,
@Longitude decimal(18, 2) null,
@Latitude decimal(18, 2) null
as begin
if @Mode='create'
begin
insert into Result_Master(Course_Id,Sem_Id,Student_Id,Exam_Id,Subject_Id,Obtained_Marks,Total_Marks,Created_By,Created_Date,Longitude,Latitude,Obsolete)
values(@Course_Id,@Sem_Id,@Student_Id,@Exam_Id,@Subject_Id,@Obtained_Marks,@Total_Marks,@Created_By,getdate(),@Longitude,@Latitude,'N')
end
else if @Mode='update'
begin
update Result_Master set Course_Id=@Course_Id,Sem_Id=@Sem_Id,Student_Id=@Student_Id,Exam_Id=@Exam_Id,Subject_Id=@Subject_Id,Obtained_Marks=@Obtained_Marks,Total_Marks=@Total_Marks,Modified_By=@Modified_By,Modified_Date=getdate(),Longitude=@Longitude,Latitude=@Latitude
where Res_Id=@Res_Id
end
else if @Mode='read'
begin
select * from Result_Master where Res_Id=@Res_Id
end
else if @Mode='delete'
begin
update Result_Master set Obsolete='O',Modified_By=@Modified_By,Modified_Date=getdate() where Res_Id=@Res_Id
end
end


select * from Result_Master