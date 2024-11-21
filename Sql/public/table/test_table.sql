create table test_table
(
    id bigint primary key generated always as identity, 
    data character varying
);