CREATE USER starfleet;
CREATE DATABASE starfleet;

GRANT ALL PRIVILEGES ON DATABASE starfleet TO starfleet;
GRANT ALL PRIVILEGES ON DATABASE starfleet TO postgres;

\connect starfleet

CREATE TABLE CrewMembers
    (Id INT PRIMARY KEY,
    Name VARCHAR(30));

INSERT INTO CrewMembers 
    VALUES
    (0, 'Patrick Picard'),
    (1, 'William Riker'),
    (2, 'Geordi Laforge'),
    (3, 'Data'),
    (4, 'Beverly Crusher');
