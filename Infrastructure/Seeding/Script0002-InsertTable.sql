INSERT INTO product (id,title,description,isActive,unitAmount,unitPrice)
VALUES ('1', 'The Last of Us', 'Very nice game with a lot of gaming features',1,30,69.90);

INSERT INTO product (id,title,description,isActive,unitAmount,unitPrice)
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