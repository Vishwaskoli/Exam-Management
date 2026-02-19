select * from Semester_Master
dbcc checkident('Semester_Master',reseed,8)
update Semester_Master set Obsolete='N'

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
    @Modified_By INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- ================= CREATE =================
    IF @Mode = 'Create'
    BEGIN
        IF EXISTS (SELECT 1 FROM Semester_Master WHERE Sem_Name = @Sem_Name AND Obsolete = 'N')
        BEGIN
            RAISERROR('Semester already exists.',16,1)
            RETURN
        END

        INSERT INTO Semester_Master (Sem_Name, Created_By, Created_Date, Obsolete)
        VALUES (@Sem_Name, @Created_By, GETDATE(), 'N')

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
    END

    -- ================= UPDATE =================
    ELSE IF @Mode = 'Update'
    BEGIN
        UPDATE Semester_Master
        SET Sem_Name = @Sem_Name,
            Modified_By = @Modified_By,
            Modified_Date = GETDATE()
        WHERE Sem_Id = @Sem_Id

        SELECT @@ROWCOUNT AS AffectedRows
    END

    -- ================= DELETE (Soft) =================
    ELSE IF @Mode = 'Delete'
    BEGIN
        UPDATE Semester_Master
        SET Obsolete = 'O',
            Modified_By = @Modified_By,
            Modified_Date = GETDATE()
        WHERE Sem_Id = @Sem_Id

        SELECT @@ROWCOUNT AS AffectedRows
    END
END

