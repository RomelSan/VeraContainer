# Vera Container
## Description
Creates virtual disks easily.  
You can create virtual disks inside your pendrive or anywhere else.  
Also you can use bitlocker to encrypt your virtual disks.  
So your personal documents & files will stay secure.

## Example usage
1. Create Virtual Disk inside your flashdrive
2. Use bitlocker to encrypt the virtual disk
3. Use it, and unmount after use.

## Technologies:
Visual Studio 2017 v15.8  
Net Framework 4.7.2  

## Requirements
Windows 10 Pro  
Net Framework 4.7.2 (Runtime)

## Commands using diskpart
***Create Virtual Disk***
```
diskpart
create vdisk file="C:\Secret.vhdx" maximum=1000
attach vdisk
create partition primary
assign letter=V
format fs=ntfs quick
exit
```

***Detach***
```
diskpart
select vdisk file="C:\Secret.vhdx"
detach vdisk
exit
```

***Attach***
```
diskpart
select vdisk file="C:\Secret.vhdx"
attach vdisk
exit
```

***Change Volume letter***  
Proceed with caution
```
diskpart
list volume
select volume 4
assign letter=X
exit
```
***Other commands***  
After step: `select vdisk file="C:\Secret.vhdx"`
* detail vdisk
* attach vdisk readonly

## License
MIT License
