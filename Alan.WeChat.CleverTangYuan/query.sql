
/*
--Initialize
if exists(select * from sys.tables  where name = 'WeChatCategories')
drop table WeChatCategories
go
create table WeChatCategories
(
	Id int identity(1,1) primary key,
	Name nvarchar(500) not null
)
go
if exists(select * from sys.tables  where name = 'WeChatUsers')
drop table WeChatUsers
go
create table WeChatUsers(
	Id int identity(1,1) primary key,
	OpenId varchar(100) not null,
	Category nvarchar(500) null,
	CreateDate datetime not null,
	NickName nvarchar(500) null,
	Gender int null,
	City nvarchar(50) null,
	Province nvarchar(50) null,
	Avatar varchar(100) null
)

go
if exists(select * from sys.tables  where name = 'WeChatUserRecords')
drop table WeChatUserRecords
go
create table WeChatUserRecords(
	Id int identity(1,1) primary key,
	OpenId varchar(100) not null,
	CreateDate datetime not null,
	Category nvarchar(500) null,
	Content nvarchar(max) null
)

*/

select * from WeChatUsers 
select * from WeChatUserRecords