create database CrudADOdb;

create table Employees(
Id int primary key identity(1,1),
name varchar(50) not null,
gender char(1) not null,
age int not null,
designation varchar(50) not null
)

select * from Employees

go
create proc spAddEmployee
(
@name varchar(50),
@gender char(1),
@age int,
@designation varchar(50)
)
as begin
insert into Employees(name,gender,age,designation)
values(@name,@gender,@age,@designation)
end

go
create proc spUpdateEmployee
(
@Id int,
@name varchar(50),
@gender char(1),
@age int,
@designation varchar(50)
)
as begin
update Employees set name = @name ,gender = @gender,age=@age,designation=@designation
where Id = @Id
end

go 
create proc spDeleteEmployees
@Id int 
as begin 
delete from Employees where Id = @Id
end

go 
create proc spGetAllEmployees
@Id int
as begin 
select * from Employees order by Id
end