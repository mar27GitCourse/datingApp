import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(), // to ensure it loads up with the root module
    ToastrModule.forRoot(
      { positionClass: 'toast-bottom-right' }
    )
  ],
  exports: /// we need to export the module to allow them to be available everywere (the configuration is not neede to expor like .forRoot() )
    [
      BsDropdownModule,
      ToastrModule
    ]
})
export class SharedModule { }
