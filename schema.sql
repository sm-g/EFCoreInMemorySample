create table import(
    id int not null auto_increment,
    start datetime,

    primary key(id)
);

create table import_customer(
    id int not null auto_increment,
    import_id int not null,
    customer_id int not null,
    name varchar(191) not null,

    primary key(id),
    constraint fk__import_customer__import
        foreign key (import_id) references import (id)
        on delete cascade on update cascade
);

CREATE UNIQUE INDEX ak__import_customer
    ON import_customer (import_id,customer_id);
    
create table import_project(
    id int not null auto_increment,
    import_id int not null,
    project_id int not null,
    name varchar(191) not null,
    import_customer_id int not null,

    primary key(id),
    constraint fk__import_project__import
        foreign key (import_id) references import (id)
        on delete cascade on update cascade,
    constraint fk__import_project__import_customer
        foreign key (import_customer_id) references import_customer (id)
        on delete cascade on update cascade
);

CREATE UNIQUE INDEX ak__import_project
    ON import_project (import_id,project_id);
    
create table import_group(
    id int not null auto_increment,
    import_id int not null,
    group_id int not null,
    name varchar(40) not null,

    primary key(id),
    constraint fk__import_group
        foreign key (import_id) references import (id)
        on delete cascade on update cascade
);

CREATE UNIQUE INDEX ak__import_group
    ON import_group (import_id,group_id);

create table import_zone(
    id int not null auto_increment,
    import_id int not null,
    zone_id int not null,
    name varchar(40) not null,

    primary key(id),
    constraint fk__import_zone__import
        foreign key (import_id) references import (id)
        on delete cascade on update cascade
);

CREATE UNIQUE INDEX ak__import_zone
    ON import_zone (import_id,zone_id);

create table import_user(
    id int not null auto_increment,
    import_id int not null,
    user_id int not null,
    first_name varchar(32) not null,
    middle_name varchar(32) not null,
    last_name varchar(32) not null,
    import_group_id int,
    import_zone_id int,

    primary key(id),
    constraint fk__import_user__import
        foreign key (import_id) references import (id)
        on delete cascade on update cascade,
    constraint fk__import_user__import_group
        foreign key (import_group_id) references import_group (id)
        on delete cascade on update cascade,
     constraint fk__import_user__import_zone
        foreign key (import_zone_id) references import_zone (id)
        on delete cascade on update cascade
);

CREATE UNIQUE INDEX ak__import_user
    ON import_user (import_id,user_id);