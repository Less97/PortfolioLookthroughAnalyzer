#!/bin/bash

# Portfolio Analyzer Startup Script
# This script starts both the API and Web projects

echo "🚀 Starting Portfolio Analyzer..."
echo ""

# Get the script directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Stop any existing processes first
echo "🛑 Stopping any existing processes..."
"$SCRIPT_DIR/stop.sh" > /dev/null 2>&1
sleep 2

# Clean and rebuild everything first
echo "🧹 Cleaning old builds..."
cd "$SCRIPT_DIR"
dotnet clean > /dev/null 2>&1
rm -rf src/*/bin src/*/obj

echo "🔨 Building solution..."
dotnet build
if [ $? -ne 0 ]; then
    echo "❌ Build failed! Check errors above."
    exit 1
fi

# Create logs directory
mkdir -p "$SCRIPT_DIR/logs"

# Start API
echo "📡 Starting API service..."
cd "$SCRIPT_DIR/src/PortfolioAnalyzer.Api"
dotnet run --no-build > "$SCRIPT_DIR/logs/api.log" 2>&1 &
API_PID=$!
echo "   API PID: $API_PID"

# Wait a bit for API to start
sleep 3

# Start Web
echo "🌐 Starting Web application..."
cd "$SCRIPT_DIR/src/PortfolioAnalyzer.Web"
dotnet run --no-build > "$SCRIPT_DIR/logs/web.log" 2>&1 &
WEB_PID=$!
echo "   Web PID: $WEB_PID"

# Save PIDs to file for stop script
echo "$API_PID" > "$SCRIPT_DIR/.api.pid"
echo "$WEB_PID" > "$SCRIPT_DIR/.web.pid"

echo ""
echo "⏳ Waiting for services to start..."
sleep 8

echo ""
echo "✅ Portfolio Analyzer is running!"
echo ""
echo "📊 Dashboard: http://localhost:5001"
echo "🔌 API:       http://localhost:5000"
echo "📋 Swagger:   http://localhost:5000/swagger"
echo ""
echo "📝 Logs:"
echo "   API: $SCRIPT_DIR/logs/api.log"
echo "   Web: $SCRIPT_DIR/logs/web.log"
echo ""
echo "🛑 To stop: ./stop.sh"
echo ""

# Open browser (macOS)
sleep 2
if command -v open &> /dev/null; then
    echo "🌍 Opening browser..."
    open http://localhost:5001
fi

echo ""
echo "Press Ctrl+C to view this information again, or use ./stop.sh to stop all services."
echo ""

# Keep script running
tail -f /dev/null
