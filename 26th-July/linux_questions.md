
### You have a file with permissions -rw-r--r--, and you run chmod +x file.sh. What happens?

- The command will add execute permissions to all users (owner, group, and others), changing the permissions to -rwxr-xr-x

### What is the difference between chmod 744 file.txt and chmod u=rwx,go=r file.txt?

- Both commands grant read, write, and execute permissions to the owner and only read permissions to group and others. The difference is in notation: 744 uses octal notation while u=rwx,go=r uses symbolic notation.

### What is the sticky bit, and when should you use it?

- The sticky bit is a permission that ensures only the file owner (or root) can delete or rename the file, even if others have write permissions to the directory. It's commonly used on directories like /tmp and is set using chmod +t.

### You are told to give the owner full access, group only execute, and others no permissions. What symbolic command achieves this?

- chmod u=rwx,g=x,o= file.txt

### What is umask, and why is it important?

- umask is the default permission mask that determines the initial permissions assigned to newly created files and directories. It's important because it controls the security baseline for new files in the system.

### If the umask is 022, what are the default permissions for a new file and a new directory?

- With umask 022, new files get permissions rw-r--r-- (644) and new directories get permissions rwxr-xr-x (755).

### Why is umask often set to 002 in development environments but 027 or 077 in production?

- In development environments, umask 002 allows group members to read and write files, facilitating collaboration. In production, umask 027 allows group read access but no permissions for others, while umask 077 restricts all permissions to the owner only, providing maximum security.


### Adduser vs Useradd?

- Both commands create new users, but useradd is a low-level utility that requires manual configuration of passwords and home directories, while adduser is a more user-friendly script that interactively prompts for passwords and automatically sets up the user's home directory and default settings.