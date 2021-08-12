# Alpha-Control
Command a squad without direct control to accomplish missions

Version: 0.00001 Starting version /w placeholders

Target Features for next version:
* Movement system
  - [x] Moves toward point (Currently with Right Click)
  - [x] Shows move path (Refer to Command & Conquer games)
  - [ ] Smart positioning path for multiple units (Refer to Command & Conquer games)
  - [ ] Unit formations (Refer to Warcraft 3)
  - [ ] Hold position
  
* Shooting system
  - [ ] Shooting opponents on sight
  - [ ] Sight system per unit 
  - [ ] Reload system
  
* Behavior system
  - [ ] Hold fire / Open fire
  - [ ] Alert stance (Units rotate to maximize vision)
  - [ ] Charging stance (Units rotate towards move path)
  
* Game rules
  - [ ] Shooting accuracy is low when moving and high when holding position
  - [ ] Units move faster in Charging stance and slower when on Alert
  - [ ] Units take more damage when reloading


Changelog 
8/12/21
* Added line formation but only for 3 units, still looking for heuristic to optimize target paths

Prev:
* Abstracted Map and Group class from Game and prepared class diagrams in PlantUML format
* Trying out Git Merge feature by adding pull request for Enemy Detection System while waiting for Smart Positioning System, will update Changelog after merge
* Added collision to units
* Added move path during move command
