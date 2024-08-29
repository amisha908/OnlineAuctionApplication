import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  username: string | null = null;
  isLoggedIn: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe(isAuthenticated => {
      this.isLoggedIn = isAuthenticated;
      if (isAuthenticated) {
        this.authService.token$.subscribe(token => {
          if (token) {
            try {
              const decodedToken: any = jwtDecode(token);
              this.username = decodedToken.sub[1] || 'User'; // Extracting User1 from the "sub" array
            } catch (e) {
              console.error('Error decoding token:', e);
              this.username = 'User';
            }
          }
        });
      } else {
        this.username = null;
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
