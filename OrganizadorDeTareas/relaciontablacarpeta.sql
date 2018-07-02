ALTER TABLE Tarea
ADD CONSTRAINT FK_TareaCarpeta
FOREIGN KEY (idCarpeta) REFERENCES Carpeta(idCarpeta)