#!/bin/bash
# Setup React Vite + TypeScript + shadcn/ui for ZKTeco Client

echo "================================================"
echo "ZKTeco Client - React Vite Setup"
echo "================================================"
echo ""

# 1. Create Vite React TypeScript project
echo "📦 Creating Vite React TypeScript project..."
npm create vite@latest zkteco-client -- --template react-ts

cd zkteco-client

# 2. Install dependencies
echo ""
echo "📦 Installing dependencies..."
npm install

# 3. Install Tailwind CSS
echo ""
echo "🎨 Installing Tailwind CSS..."
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# 4. Install shadcn/ui dependencies
echo ""
echo "🎨 Installing shadcn/ui dependencies..."
npm install class-variance-authority clsx tailwind-merge
npm install lucide-react
npm install @radix-ui/react-slot
npm install -D @types/node

# 5. Install other required dependencies
echo ""
echo "📦 Installing additional dependencies..."
npm install react-router-dom
npm install @tanstack/react-query
npm install axios
npm install date-fns
npm install recharts
npm install react-hook-form @hookform/resolvers zod
npm install sonner # Toast notifications

echo ""
echo "✅ All dependencies installed!"
echo ""
echo "================================================"
echo "Next Steps:"
echo "================================================"
echo ""
echo "1. Configure Tailwind CSS (see tailwind.config.js artifact)"
echo "2. Update tsconfig files"
echo "3. Add shadcn/ui components"
echo "4. Run: npm run dev"
echo ""

# Create necessary directories
mkdir -p src/components/ui
mkdir -p src/lib
mkdir -p src/hooks
mkdir -p src/services
mkdir -p src/types
mkdir -p src/pages
mkdir -p src/layouts

echo "📁 Project structure created!"
echo ""
echo "Run these commands:"
echo "  cd zkteco-client"
echo "  npm run dev"