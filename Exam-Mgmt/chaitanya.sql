select * from Semester_Master
dbcc checkident('Semester_Master',reseed,8)
update Semester_Master set Obsolete='N'

truncate table Semester_Master

set identity_insert Semester_Master off
delete from Semester_Master where Sem_Name='IT'
go
insert into Semester_Master
(Sem_Name,Created_By)
values
('IT',1)

go

create proc sp_CreateSemester
@Sem_Name varchar(50),
@Created_By int
as
begin
insert into Semester_Master(Sem_Name,Created_By)
values(@Sem_Name,@Created_By)
end
go



create proc sp_GetAllSemester
as 
begin
select Sem_Id,Sem_Name,Created_By,Created_Date,Modified_By,Modified_Date,Obsolete from dbo.Semester_Master
end
go


CREATE OR ALTER PROC sp_SemesterCRUD
    @Mode VARCHAR(20),

    @Sem_Id INT = NULL,
    @Sem_Name VARCHAR(50) = NULL,

    @Created_By INT = NULL,
    @Modified_By INT = NULL,

    @Latitude DECIMAL(18,2) = NULL,
    @Longitude DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- ================= CREATE =================
    IF @Mode = 'Create'
    BEGIN
        IF EXISTS (
            SELECT 1 
            FROM Semester_Master 
            WHERE Sem_Name = @Sem_Name 
              AND Obsolete = 'N'
        )
        BEGIN
            RAISERROR('Semester already exists.',16,1)
            RETURN
        END

        INSERT INTO Semester_Master
        (
            Sem_Name,
            Created_By,
            Created_Date,
            Obsolete,
            Latitude,
            Longitude
        )
        VALUES
        (
            @Sem_Name,
            @Created_By,
            GETDATE(),
            'N',
            @Latitude,
            @Longitude
        )

        SELECT SCOPE_IDENTITY() AS NewId
    END

    -- ================= READ ALL =================
    ELSE IF @Mode = 'Read'
    BEGIN
        SELECT *
        FROM Semester_Master
        WHERE Obsolete = 'N'
    END

    -- ================= READ BY ID =================
    ELSE IF @Mode = 'ReadById'
    BEGIN
        SELECT *
        FROM Semester_Master
        WHERE Sem_Id = @Sem_Id
          AND Obsolete = 'N'
    END

    -- ================= UPDATE =================
    ELSE IF @Mode = 'Update'
    BEGIN
        IF NOT EXISTS (
            SELECT 1 
            FROM Semester_Master 
            WHERE Sem_Id = @Sem_Id 
              AND Obsolete = 'N'
        )
        BEGIN
            RAISERROR('Semester not found.',16,1)
            RETURN
        END

        UPDATE Semester_Master
        SET
            Sem_Name = @Sem_Name,
            Modified_By = @Modified_By,
            Modified_Date = GETDATE(),
            Latitude = @Latitude,
            Longitude = @Longitude
        WHERE Sem_Id = @Sem_Id

        SELECT @@ROWCOUNT AS AffectedRows
    END

    -- ================= DELETE (Soft Delete) =================
    ELSE IF @Mode = 'Delete'
    BEGIN
        UPDATE Semester_Master
        SET
            Obsolete = 'O',
            Modified_By = @Modified_By,
            Modified_Date = GETDATE()
        WHERE Sem_Id = @Sem_Id

        SELECT @@ROWCOUNT AS AffectedRows
    END
END
GO

CREATE TABLE dbo.Semester_Master
(
    Sem_Id INT IDENTITY(1,1) PRIMARY KEY,
    Sem_Name VARCHAR(50) NOT NULL,
    Created_By INT NOT NULL,
    Created_Date DATETIME NOT NULL DEFAULT GETDATE(),
    Modified_By INT NULL,
    Modified_Date DATETIME NULL,
    Obsolete CHAR(1) NOT NULL DEFAULT 'N'
);
GO

alter table Semester_Master add Latitude decimal(18,2),
Longitude decimal(18,2)







create table Result_Master
(
Result_Id int identity(1,1) primary key,
Course_Id int not null,
Sem_Id int not null,
Student_Id int not null,
Exam_Id int not null,
Subject_Id int not null,
Total_Marks int not null,
Obtained_Marks int not null,
Created_By int not null,
Created_Date datetime not null default getdate(),
Modified_By int null,
Modified_Date datetime null,
Longitude decimal(18,2) null,
Latitude decimal(18,2) null,
Obsolete char(1) not null default 'N'
)

select * from Result_Master