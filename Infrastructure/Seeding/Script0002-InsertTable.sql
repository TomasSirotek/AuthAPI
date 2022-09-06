--User insert

INSERT INTO app_user (id,firstName,lastName,email,passwordHash,isActivated,createdAt)
VALUES ('a8bd0476-eb11-46f4-9d83-bd5e44f8637f','Admin', 'LastName','google@yahoo.com','AQAAAAEAACcQAAAAELy3B0Gbw26KFuOJPKsZNFpIzvw1dIBI8BzOsduE7rfm3nN4vvOXkPBJdYF0sqINRg==',1,'2020-02-03');

INSERT INTO role(id,name)
VALUES ('300','Administrator');

INSERT INTO role(id,name)
VALUES ('301','User');

INSERT INTO user_role(userId,roleId)
VALUES ('a8bd0476-eb11-46f4-9d83-bd5e44f8637f','300');

INSERT INTO user_role(userId,roleId)
VALUES ('a8bd0476-eb11-46f4-9d83-bd5e44f8637f','301');

INSERT INTO address(id,userId,street,number,country,zip)
VALUES ('a11de476-eb01-44f2-9d83-bd5e44f8637f','a8bd0476-eb11-46f4-9d83-bd5e44f8637f','Vesterhavsgade',15,'Denmark',6700);

