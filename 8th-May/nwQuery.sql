select o.OrderId, o.OrderDate, e.FirstName, c.CompanyName from Orders o JOIN Employees e ON o.EmployeeID = e.EmployeeID JOIN Customers c ON c.CustomerID = o.CustomerID;

select p.ProductName, c.CategoryName, s.CompanyName as suppliers from Products p JOIN Categories c on p.CategoryID = c.CategoryId JOIN Suppliers s ON s.SupplierID = p.SupplierID;

select o.OrderID, od.UnitPrice, od.Quantity, p.ProductName from Orders o JOIN [Order Details] od ON o.OrderID = od.OrderID JOIN Products p ON p.ProductID = od.ProductID order by o.OrderID;


SELECT e.FirstName as manager, e2.FirstName as emp FROM Employees e JOIN Employees e2 ON e2.ReportsTo = e.EmployeeID;


SELECT c.CompanyName , Count(o.OrderID) as [No of Orders] from Customers c JOIN Orders o ON o.CustomerID = c.CustomerID GROUP BY c.CompanyName;

SELECT c.CategoryName, AVG(p.UnitPrice) as [avg unit price] from Products p JOIN Categories c ON c.CategoryID = p.CategoryID GROUP BY CategoryName;

SELECT CustomerID, CompanyName FROM Customers WHERE ContactTitle LIKE 'Owner';

SELECT TOP 5 * FROM Products Order By UnitPrice DESC;

SELECT OrderID,SUM(Quantity * UnitPrice) AS [sales value] FROM [Order Details] group by OrderID;

create or alter proc orderstuff(@CustomerID varchar(20))
as
begin
	SELECT * FROM Orders Where CustomerId = @CustomerID;
end

sp_help Orders

orderstuff 'VINET'

select * from Orders;

create or alter proc insertstuff(@pname nvarchar, @sid int, @cid int, @uprice int)
as
begin
	INSERT INTO Products VALUES(@pname, @sid, @cid, '5 kg pkg.', @uprice, 15, 20, 16, 0);
end

insertstuff 'jiji',18,4,18

sp_help Products;

select * from Products where SupplierID=4;


create or alter proc totalsales
as 
begin 
	SELECT e.EmployeeID, e.FirstName, SUM(od.Quantity * od.UnitPrice) AS [total sales] FROM Employees e JOIN Orders o
	ON o.EmployeeID = e.EmployeeID JOIN [Order Details] od ON od.OrderID = o.OrderID GROUP BY e.EmployeeID, e.FirstName, e.LastName;
end

totalsales