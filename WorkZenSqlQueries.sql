create table Department(
	id int identity(1,1) not null,
	name varchar(80) not null,
	constraint pk_depId primary key(id)
);

create table Employee(
	id int identity(1,1) not null,
	employeeCode varchar(200) not null,
	email varchar(100) not null,
	password varchar(20) not null,
	firstName varchar(40) not null,
	lastName varchar(40) not null,
	birthDate date not null,
	gender char(6) not null,
	departmentId int references Department(id) on delete set null default 1,
	reportingPerson int references Employee(id) on delete no action,
	constraint pk_employeeId primary key(id)
);

create table Task(
	id int identity(1,1) not null,
	taskDate date not null,
	employeeId int references Employee(id) on delete cascade,
	taskName varchar(100) not null,
	taskDescription text not null,
	approverId int references Employee(id),
	approvedOrRejectedBy int references Employee(id),
	status char(15) not null,
	createdOn date not null default GETDATE(),
	modifiedOn date not null default GETDATE(),
	constraint pk_taskId primary key(id)
);

drop table Department

select * from department

select * from Employee

select * from Task

exec sp_rename 'Task.tastDescription', 'taskDescription', 'column'

select * from Department

insert into Department(name) values ('Employee'), ('Manager'), ('Director')

update Employee set departmentId = 3
where employeeCode = 'WorkZen4'

update Employee set reportingPerson = 4
where employeeCode = 'WorkZen4'

update Employee set email = 'mitrao@outlook.com'
where employeeCode = 'WorkZen4'

delete from Employee where id = 3

delete from Employee where id = 5

use MitRao

update Employee set departmentId = 1 where firstName = 'john'




--procedure for updating an employee

create or alter proc sp_update_employee
@id int, @employeeCode varchar(30), @email varchar(80), @password varchar(100), @firstName varchar(30), @lastName varchar(30), @birthDate date, @gender char(6), @departmentId int, @reportingPerson int
as
Begin
	update Employee set
	employeeCode = @employeeCode,
	email = @email,
	password = @password,
	firstName = @firstName,
	lastName = @lastName,
	birthDate = @birthDate,
	gender = @gender,
	departmentId = @departmentId,
	reportingPerson = @reportingPerson
	where id = @id
end

