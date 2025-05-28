import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  profileImage = 'assets/images/profile-avatar.jpg';
  showProfile = false;
  showPasswordForm = false;
  currentPassword = '';
  newPassword = '';
  confirmPassword = '';

  constructor(private router: Router) {}

  toggleProfile() {
    this.showProfile = !this.showProfile;
    this.showPasswordForm = false;
  }

  changePassword() {
    if (this.newPassword === this.confirmPassword) {
      // Logique pour changer le mot de passe
      console.log('Mot de passe changé');
      this.showPasswordForm = false;
      this.currentPassword = '';
      this.newPassword = '';
      this.confirmPassword = '';
    } else {
      console.error('Les mots de passe ne correspondent pas');
    }
  }

  logout() {
    // Logique de déconnexion
    this.router.navigate(['/login']);
  }
}