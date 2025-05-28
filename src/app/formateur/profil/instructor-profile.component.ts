// src/app/components/instructor-profile/instructor-profile.component.ts

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { InstructorService } from 'src/app/services/instructor.service';
import { Instructor, CreateInstructorDTO, UpdateInstructorDTO } from 'src/app/models/instructor.model';

@Component({
  selector: 'app-instructor-profile',
  templateUrl: './instructor-profile.component.html',
  styleUrls: ['./instructor-profile.component.css']
})
export class InstructorProfileComponent implements OnInit {
  instructors: Instructor[] = [];
  instructor: Instructor | null = null;
  errorMessage: string = '';
  successMessage: string = '';
  isLoading = false;
  isEditing = false;

  profileForm: FormGroup = this.fb.group({});

  constructor(
    private instructorService: InstructorService,
    private fb: FormBuilder
  ) {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]], // 🔥 Ajouté ici
      cin: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(10)]],
      telephone: ['', Validators.pattern(/^[0-9]{10}$/)],
      specialite: ['', Validators.required],
      address: ['']
    });
  }

  ngOnInit() {
    this.loadInstructors();
  }

  initNewInstructor() {
    this.instructor = null;
    this.isEditing = true;
    this.profileForm.reset();
  }

  loadInstructors() {
    this.isLoading = true;
    this.instructorService.getAllInstructors().subscribe({
      next: (data) => {
        this.instructors = data;
        if (!this.instructor && data.length > 0) {
          this.selectInstructor(data[0].id); // Auto-select first
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load instructors.';
        this.isLoading = false;
        console.error(error);
      }
    });
  }

  onSubmit() {
    if (this.profileForm.invalid) {
      this.markFormGroupTouched(this.profileForm);
      this.errorMessage = 'Please correct the validation errors.';
      return;
    }

    this.isLoading = true;
    const formValue = this.profileForm.value;

    if (this.isEditing && this.instructor) {
      this.updateInstructor(formValue);
    } else {
      this.createInstructor(formValue);
    }
  }

  private createInstructor(dto: CreateInstructorDTO) {
    this.instructorService.createInstructor(dto).subscribe({
      next: (data) => {
        this.handleSuccess('Instructor created successfully.');
        this.instructor = data;
      },
      error: (err) => this.handleError(err, 'Failed to create instructor.')
    });
  }

  private updateInstructor(dto: UpdateInstructorDTO) {
    if (!this.instructor) return;
    this.instructorService.updateInstructor(this.instructor.id, dto).subscribe({
      next: (data) => {
        this.handleSuccess('Instructor updated successfully.');
        this.instructor = data;
      },
      error: (err) => this.handleError(err, 'Failed to update instructor.')
    });
  }

  private handleSuccess(message: string) {
    this.successMessage = message;
    this.errorMessage = '';
    this.isLoading = false;
    this.isEditing = false;
    this.loadInstructors();
  }

  private handleError(error: HttpErrorResponse, defaultMessage: string) {
    if (error.status === 400 && error.error.errors) {
      this.errorMessage = this.formatValidationErrors(error.error.errors);
    } else {
      this.errorMessage = error.error?.message || defaultMessage;
    }
    this.isLoading = false;
    console.error(error);
  }

  private formatValidationErrors(errors: any): string {
    return Object.keys(errors)
      .map(key => `${key}: ${errors[key].join(', ')}`)
      .join('\n');
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getFieldError(field: string): string {
    const control = this.profileForm.get(field);
    if (!control || !control.touched || !control.errors) return '';

    if (control.hasError('required')) return 'This field is required.';
    if (control.hasError('email')) return 'Invalid email format.';
    if (control.hasError('minlength'))
      return `Minimum length is ${control.errors['minlength'].requiredLength}.`;
    if (control.hasError('maxlength'))
      return `Maximum length is ${control.errors['maxlength'].requiredLength}.`;
    if (control.hasError('pattern')) return 'Invalid format.';

    return '';
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
    if (this.instructor && !this.isEditing) {
      this.loadInstructorData(this.instructor);
    }
  }

  selectInstructor(id: number) {
    this.instructorService.getInstructorById(id).subscribe({
      next: (data) => {
        this.instructor = data;
        this.loadInstructorData(data);
        this.isEditing = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load instructor details.';
        console.error(error);
      }
    });
  }

  deleteInstructor(id: number) {
    if (confirm('Are you sure you want to delete this instructor?')) {
      this.instructorService.deleteInstructor(id).subscribe({
        next: () => {
          this.successMessage = 'Instructor deleted successfully.';
          if (this.instructor?.id === id) {
            this.instructor = null;
            this.profileForm.reset();
          }
          this.loadInstructors();
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete instructor.';
          console.error(error);
        }
      });
    }
  }

  private loadInstructorData(instructor: Instructor) {
    this.profileForm.patchValue({
      firstName: instructor.firstName,
      lastName: instructor.lastName,
      email: instructor.email,
      cin: instructor.cin,
      telephone: instructor.telephone,
      specialite: instructor.specialite,
      address: instructor.address
    });
  }
}