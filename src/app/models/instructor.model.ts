// src/app/models/instructor.model.ts

export interface Instructor {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  cin: string;
  telephone: string;
  specialite: string;
  address: string;
}

export interface CreateInstructorDTO {
  firstName: string;
  lastName: string;
  email: string;
  password: string; // 🔥 Ajouté ici
  cin: string;
  telephone: string;
  specialite: string;
  address: string;
}

export interface UpdateInstructorDTO {
  firstName?: string;
  lastName?: string;
  email?: string;
  cin?: string;
  telephone?: string;
  specialite?: string;
  address?: string;
}