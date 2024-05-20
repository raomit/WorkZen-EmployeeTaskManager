use MitRao


exec get_paged_all_employee_tasks 0,5
CREATE OR ALTER PROC get_paged_all_employee_tasks
@offset INT, 
@size INT,
@order_dir NVARCHAR(50) = null,
@order_column NVARCHAR(50) = null
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
	if @order_dir is null or @order_column is null
	begin
		SET @sql = N'SELECT t.id, t.taskDate, t.employeeId, t.taskName, t.taskDescription, t.approverId, t.approvedOrRejectedBy, t.status, t.createdOn, t.modifiedOn 
                FROM Task t 
                JOIN Employee e ON t.employeeId = e.id
                WHERE e.departmentId = 1
                ORDER BY t.id
                OFFSET ' + CAST(@offset AS NVARCHAR(10)) + ' ROWS FETCH NEXT ' + CAST(@size AS NVARCHAR(10)) + ' ROWS ONLY;'
	end
	else if @order_dir = 'asc'
	begin
	print(@order_column)
		SET @sql = N'SELECT t.id, t.taskDate, t.employeeId, t.taskName, t.taskDescription, t.approverId, t.approvedOrRejectedBy, t.status, t.createdOn, t.modifiedOn 
                FROM Task t 
                JOIN Employee e ON t.employeeId = e.id
                WHERE e.departmentId = 1
                ORDER BY t.' + cast(@order_column as varchar) +
                ' OFFSET ' + CAST(@offset AS NVARCHAR(10)) + ' ROWS FETCH NEXT ' + CAST(@size AS NVARCHAR(10)) + ' ROWS ONLY;'
	end
	else
	begin
		SET @sql = N'SELECT t.id, t.taskDate, t.employeeId, t.taskName, t.taskDescription, t.approverId, t.approvedOrRejectedBy, t.status, t.createdOn, t.modifiedOn 
                FROM Task t 
                JOIN Employee e ON t.employeeId = e.id
                WHERE e.departmentId = 1
                ORDER BY t.' + cast(@order_column as varchar) + ' desc' +
                ' OFFSET ' + CAST(@offset AS NVARCHAR(10)) + ' ROWS FETCH NEXT ' + CAST(@size AS NVARCHAR(10)) + ' ROWS ONLY;'
	end
    
    EXEC sp_executesql @sql;
END

use MitRao


select * from Task

exec get_paged_all_employee_tasks 0, 10, 'asc', 'taskDescription'








