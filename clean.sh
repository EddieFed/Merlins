#!/bin/bash

echo "Current directory: $(pwd)"
echo "This will delete some files within this directory! Make sure Unity is not running!"
read -p "Press Enter to continue..."

echo "Are you sure you would like to do this? (y/n)"
read -r confirmation
if [[ $confirmation != "y" ]]; then
  echo "Operation cancelled."
  exit 1
fi

rm -rf Library
rm -rf Logs
rm -rf obj
rm -rf Temp
rm -rf .idea
rm -rf .vscode
rm -rf Builds
find . -type f -name "*.csproj" -exec rm -f {} +
find . -type f -name "*.pidb" -exec rm -f {} +
find . -type f -name "*.unityproj" -exec rm -f {} +
find . -type f -name "*.DS_Store" -exec rm -f {} +
find . -type f -name "*.sln" -exec rm -f {} +
find . -type f -name "*.userprefs" -exec rm -f {} +

echo "Done."
read -p "Press Enter to exit..."
