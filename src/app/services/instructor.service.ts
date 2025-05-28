// src/app/services/instructor.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Instructor, CreateInstructorDTO, UpdateInstructorDTO } from '../models/instructor.model';

@Injectable({
  providedIn: 'root'
})
export class InstructorService {
  private apiUrl = 'https://localhost:7091/api/Instructors';

  constructor(private http: HttpClient) {}

  getAllInstructors(): Observable<Instructor[]> {
    return this.http.get<Instructor[]>(this.apiUrl);
  }

  getInstructorById(id: number): Observable<Instructor> {
    return this.http.get<Instructor>(`${this.apiUrl}/${id}`);
  }

  createInstructor(dto: CreateInstructorDTO): Observable<Instructor> {
    return this.http.post<Instructor>(this.apiUrl, dto);
  }

  updateInstructor(id: number, dto: UpdateInstructorDTO): Observable<Instructor> {
    return this.http.put<Instructor>(`${this.apiUrl}/${id}`, dto);
  }

  deleteInstructor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}