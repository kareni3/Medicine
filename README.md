# ������� ��� ������������� �������� �������������� ���������� ���������� � ���� ������
## ���������
������ �������������� ������� � VisualStudio, NuGet ����������� ��� ����������� �� `packages.config`.
## ��� ��������
��� ��������� � �� MongoDB ���������� ������� ��������� ������ `MongoConnection` � �������� � ���� ��� �����, ����,
��� ������������ � ������. ���������������, ��� �� �����, � �������� �� �������������, ��� ���������� ���� ������
`medicine`. ����� ����� ������� ����� `MongoConnection.Connect()`:
```cs
MongoConnection connection = new MongoConnection("localhost", "27017", "user-name", "password");
connection.Connect();
```
��� ���� �������, ����� ����������, �������� ���������� �� ��������, ��������� � ��������� ��������������� ���������.
��� ���������� �������� �������� � ���������.
��� �������� ������ ��������, ��������, ����, ����� ������� ��� �����������, � ������� ����� �������� ���������� ���� �
��������� ������ `MongoConnection`. ��� ���������� ��������� � �� ����� ������� ����� `Tag.Save()`:
```cs
Tag tag = new Tag("����������������", connection);
tag.Save();
```
����� ����� ������� ����������� ��� ����������, � ����������� ���������� � ��������� `MongoConnection` ������� �����:
```cs
Tag tag = new Tag();
tag.Content = "����������������";
tag.Connection = connection;
tag.Save();
```
��� ��������� ��������� �� ���� ������ �� id ������ ����� `GetById(ObjectId, MongoConnection)`:
```cs
Doctor doctor = new Doctor();
ObjectId id = new ObjectId("1234567890ab");
doctor.GetById(id, connection);
```
����� � ���� ��������� ������� ������ `GetBy...(..., MongoConnection)`, � ������� ������� ����� �������� ������� �� ��� ����������� ��������������.
��� ��������� ��������� � �� ����� ��������� ��� ��������� ��������, � ����� ������� ����� `Save()`.
```cs
Tag tag = new Tag();
tag.GetByContent("����������������", connection);
tag.Content = "����������������";
tag.Save();
```
��� � ����� � �������� �� �������� ���������� ���������������, ������� ��� ��� � ���������� ����� ����� �������� ������ `GetByName()`.