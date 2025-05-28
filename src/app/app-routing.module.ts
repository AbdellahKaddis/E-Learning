import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmploisComponent } from './formateur/emplois/emplois.component';
import { InstructorProfileComponent } from './formateur/profil/instructor-profile.component';



const routes: Routes = [
  {path:"emplois",component:EmploisComponent},
  {path:"instructor-profile",component:InstructorProfileComponent },
  
 
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
