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

SELECT * FROM Course_Sem_Mapping WHERE Course_Id = 1

SELECT *
FROM Exam_Master

USE [Student_Management_System]
GO

/****** Object: SqlProcedure [dbo].[sp_ResultCRUD] Script Date: 28-02-2026 09:20:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create or alter proc sp_ResultCRUD
@Mode varchar(20),
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
@Longitude decimal(18,2) = null,
@Latitude decimal(18,2) = null
as
begin
    set nocount on;

    if @Mode='create'
    begin
        if exists (
            select 1 from Result_Master
            where Student_Id=@Student_Id
            and Exam_Id=@Exam_Id
            and Subject_Id=@Subject_Id
            and Obsolete='N'
        )
        begin
            throw 50001, 'Duplicate result entry.', 1;
        end

        insert into Result_Master
        (Course_Id,Sem_Id,Student_Id,Exam_Id,Subject_Id,
         Obtained_Marks,Total_Marks,Created_By,Created_Date,
         Longitude,Latitude,Obsolete)
        values
        (@Course_Id,@Sem_Id,@Student_Id,@Exam_Id,@Subject_Id,
         @Obtained_Marks,@Total_Marks,@Created_By,getdate(),
         @Longitude,@Latitude,'N');
    end

    else if @Mode='update'
    begin
        update Result_Master
        set Course_Id=@Course_Id,
            Sem_Id=@Sem_Id,
            Student_Id=@Student_Id,
            Exam_Id=@Exam_Id,
            Subject_Id=@Subject_Id,
            Obtained_Marks=@Obtained_Marks,
            Total_Marks=@Total_Marks,
            Modified_By=@Modified_By,
            Modified_Date=getdate(),
            Longitude=@Longitude,
            Latitude=@Latitude
        where Res_Id=@Res_Id;
    end

    else if @Mode='delete'
    begin
        update Result_Master
        set Obsolete='O',
            Modified_By=@Modified_By,
            Modified_Date=getdate()
        where Res_Id=@Res_Id;
    end

    else if @Mode='readall'
begin
    select 
        r.Res_Id,
        r.Course_Id,
        r.Sem_Id,
        r.Student_Id,
        r.Exam_Id,
        r.Subject_Id,
        s.Stu_First_Name + ' ' + isnull(s.Stu_Last_Name,'') as StudentName,
        e.Exam_Name,
        sub.Subject_Name,
        r.Obtained_Marks,
        r.Total_Marks
    from Result_Master r
    join Student_Master s on r.Student_Id=s.Student_Id
    join Exam_Master e on r.Exam_Id=e.Exam_Id
    join Subject_Master sub on r.Subject_Id=sub.Subject_Id
    where r.Obsolete='N';
end
end
