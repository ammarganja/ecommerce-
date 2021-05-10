import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgApexchartsModule } from 'ng-apexcharts';
import { InlineSVGModule } from 'ng-inline-svg';
// Advanced Tables
// Base Tables
import { BaseTablesWidget1Component } from './base-tables/base-tables-widget1/base-tables-widget1.component';
import { BaseTablesWidget2Component } from './base-tables/base-tables-widget2/base-tables-widget2.component';
import { BaseTablesWidget6Component } from './base-tables/base-tables-widget6/base-tables-widget6.component';
// Lists
// Mixed
import { MixedWidget1Component } from './mixed/mixed-widget1/mixed-widget1.component';
import { MixedWidget4Component } from './mixed/mixed-widget4/mixed-widget4.component';
import { MixedWidget6Component } from './mixed/mixed-widget6/mixed-widget6.component';
import { MixedWidget14Component } from './mixed/mixed-widget14/mixed-widget14.component';
// Stats
import { StatsWidget10Component } from './stats/stats-widget10/stats-widget10.component';
import { StatsWidget11Component } from './stats/stats-widget11/stats-widget11.component';
import { StatsWidget12Component } from './stats/stats-widget12/stats-widget12.component';
// Tiles
import { TilesWidget1Component } from './tiles/tiles-widget1/tiles-widget1.component';
import { TilesWidget3Component } from './tiles/tiles-widget3/tiles-widget3.component';
import { TilesWidget10Component } from './tiles/tiles-widget10/tiles-widget10.component';
import { TilesWidget11Component } from './tiles/tiles-widget11/tiles-widget11.component';
import { TilesWidget12Component } from './tiles/tiles-widget12/tiles-widget12.component';
import { TilesWidget13Component } from './tiles/tiles-widget13/tiles-widget13.component';
import { TilesWidget14Component } from './tiles/tiles-widget14/tiles-widget14.component';
// Other
import { DropdownMenusModule } from '../dropdown-menus/dropdown-menus.module';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    // Advanced Tables
    // Base Tables
    BaseTablesWidget1Component,
    BaseTablesWidget2Component,
    BaseTablesWidget6Component,
    // Lists
    // Mixed
    MixedWidget1Component,
    MixedWidget4Component,
    MixedWidget6Component,
    // Stats
    StatsWidget10Component,
    StatsWidget11Component,
    StatsWidget12Component,
    // Tiles,
    TilesWidget1Component,
    TilesWidget3Component,
    TilesWidget10Component,
    TilesWidget11Component,
    TilesWidget12Component,
    TilesWidget13Component,
    TilesWidget14Component,
    // Other
  ],
  imports: [
    CommonModule,
    DropdownMenusModule,
    InlineSVGModule,
    NgApexchartsModule,
    NgbDropdownModule,
  ],
  exports: [
    // Advanced Tables
    // Base Tables
    BaseTablesWidget1Component,
    BaseTablesWidget2Component,
    BaseTablesWidget6Component,
    // Lists
    // Mixed
    MixedWidget1Component,
    MixedWidget4Component,
    MixedWidget6Component,
    MixedWidget14Component,
    // Stats
    StatsWidget10Component,
    StatsWidget11Component,
    StatsWidget12Component,
    // Tiles,
    TilesWidget1Component,
    TilesWidget3Component,
    TilesWidget10Component,
    TilesWidget11Component,
    TilesWidget12Component,
    TilesWidget13Component,
    TilesWidget14Component,
    // Other
  ],
})
export class WidgetsModule { }
