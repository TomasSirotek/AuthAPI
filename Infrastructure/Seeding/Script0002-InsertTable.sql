--User insert

INSERT INTO app_user (id,firstName,lastName,email,passwordHash,isActivated,createdAt)
VALUES ('a8bd0476-eb11-46f4-9d83-bd5e44f8637f','Admin', 'LastName','google@yahoo.com','hashofpsw',0,'2020-02-03');

INSERT INTO role(id,name)
VALUES ('300','Administrator');

INSERT INTO role(id,name)
VALUES ('301','User');

INSERT INTO user_role(userId,roleId)
VALUES ('a8bd0476-eb11-46f4-9d83-bd5e44f8637f','300');

INSERT INTO user_role(userId,roleId)
VALUES ('a8bd0476-eb11-46f4-9d83-bd5e44f8637f','301');

--Product insert
INSERT INTO product (id,title,description,isActive,unitPrice,unitsInStock)
VALUES ('1', 'The Last of Us', 'Very nice game with a lot of gaming features',1,30,69.90);

INSERT INTO product (id,title,description,isActive,unitPrice,unitsInStock)
VALUES ('2', 'Call of Duty', 'Super action game',1,4,39.90);

INSERT INTO category(id,name)
VALUES ('100','Survival');

INSERT INTO category(id,name)
VALUES ('101','FPS');

INSERT INTO product_category(productId,categoryId)
VALUES ('1','100');

INSERT INTO product_category(productId,categoryId)
VALUES ('1','101');

INSERT INTO product_category(productId,categoryId)
VALUES ('2','100');
